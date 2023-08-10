using Google.Apis;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.GoogleAnalyticsAdmin.v1alpha;
using Google.Apis.GoogleAnalyticsAdmin.v1alpha.Data;
using Google.Apis.Services;
using Google.Apis.TagManager.v2;
using Google.Apis.TagManager.v2.Data;
using GoogleOAuthServiceTest;
using GoogleOAuthServiceTest.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;


internal class Program
{
    private static string baseUrl = "https://content-analyticsadmin.googleapis.com/v1alpha/";

    private static void Main(string[] args)
    {

        CreateGA4Properties();
    }

    public static GoogleCredential GetGoogleCredentials()
    {
        // Set up the credentials for the Google Cloud API
        return GoogleCredential.FromFile("googleauth.json").CreateScoped(
            new[] { "https://www.googleapis.com/auth/tagmanager.edit.containers" }
        );
    }

    public static string CreateGTM(long accountNumber, string containerName, string hostname)
    {
        // Get credentials
        var credentials = GetGoogleCredentials();

        // Create an instance of the TagManagerService
        var service = new TagManagerService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credentials
        });

        // Create the GTM container object
        var newContainer = new Container();
        newContainer.Name = containerName;
        newContainer.UsageContext = new List<string>();
        newContainer.DomainName = new List<string>() { hostname };

        // Create the GTM container under the existing account
        var createdContainer = service.Accounts.Containers.Create(newContainer, $"accounts/{accountNumber}").Execute();
        //return createdContainer.ContainerId;
        return createdContainer.PublicId;
    }

    /// <summary>
    /// This  method, CreateProperties(), is used to create a new Google Analytics 4 (GA4) property using the Google Analytics Admin API. 
    /// It reads the credentials from a JSON file named "googleauth.json" to establish authorization for accessing the Google Analytics Admin API with specific scopes (edit and readonly).
    /// Initializes the Google Analytics Admin API service with the provided credentials and sets the application name.
    /// Specifies the account ID, account name, and time zone for the new GA4 property
    /// Creates a new instance of GoogleAnalyticsAdminV1alphaProperty (representing the GA4 property) and sets its display name, time zone, and parent (using the account ID)
    /// Sends a request to the Analytics Admin API to create the GA4 property and executes the request. The response contains the details of the created property.
    ///  Obtains an access token from the credentials and prints a success message along with the name of the created GA4 property.
    /// Calls the method CreateWebStream() and passes the created GA4 property and access token for further processing.
    /// </summary>/
    public static void CreateGA4Properties()
    {
        // Replace "googleauth.json" with the actual path to your credentials JSON file
        var credential = GoogleCredential.FromFile("googleauth.json")
            .CreateScoped(new[] { GoogleAnalyticsAdminService.Scope.AnalyticsEdit, GoogleAnalyticsAdminService.Scope.AnalyticsReadonly });
        // Create an instance of the Analytics Admin API service
        var service = new GoogleAnalyticsAdminService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "HLM Clients - 2"
        });
        string accountId = "174569561";
        var accountName = "Demo";
        var timeZone = "America/Chicago";

        // Create a new GA4 property
        var ga4Property = new GoogleAnalyticsAdminV1alphaProperty
        {
            DisplayName = accountName,
            TimeZone = timeZone,
            Parent = $"accounts/{accountId}"
        };
        var createGa4PropertyRequest = service.Properties.Create(ga4Property);
        var createdGa4Property = createGa4PropertyRequest.Execute();
        string accessToken = credential.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        Console.WriteLine("GA property created successfully: " + createdGa4Property.Name);
        CreateWebStream(createdGa4Property, accessToken);
    }
    /// <summary>
    /// The given method CreateWebStream(), is responsible for creating a web data stream associated with a previously created Google Analytics 4 (GA4) property using the Google Analytics Admin API. The method performs the following steps:
    /// The method initializes an HTTP client to make HTTP requests.
    ///Constructs the URL for creating a new web data stream by appending the property name to a base URL.
    ///Creates an HTTP request message with the appropriate method(POST), URL, and authorization headers using the provided access token.
    ///Constructs a WebDataStreamRequest object that includes details about the web data stream to be created, such as display name, type, and default URI.
    ///If necessary, initializes the webStreamData field within the request object and sets a default URI (in this case, "https://www.google.com").
    ///Serializes the webDataStreamRequest object into JSON format and sets it as the content of the HTTP request, with appropriate content type headers.
    ///Sends the HTTP request to the API and receives the HTTP response.
    ///If the response status code indicates success:
    /// Parses the response body to extract the measurement ID of the newly created web data stream.
    ///   Prints a success message along with the extracted measurement ID.
    ///   Initiates further actions, such as sending GA4 events and marking an event conversion.
    ///If the response status code indicates an error:
    ///   Prints the error status code and reason phrase.
    ///  Reads and prints the error response body.
    /// <param name="createdGa4Property"></param>
    /// <param name="accessToken"></param>
    /// </summary>
    public static void CreateWebStream(GoogleAnalyticsAdminV1alphaProperty createdGa4Property, string accessToken)
    {
        using (var httpClient = new HttpClient())
        {
            var url = $"{baseUrl}{createdGa4Property.Name}/dataStreams";
            using var request = new HttpRequestMessage(new HttpMethod("POST"), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            WebDataStreamRequest webDataStreamRequest = new WebDataStreamRequest();
            webDataStreamRequest.DisplayName = "test";
            webDataStreamRequest.Type = "WEB_DATA_STREAM";

            if (webDataStreamRequest.webStreamData == null)
            {
                webDataStreamRequest.webStreamData = new WebRequestStreamData();
                webDataStreamRequest.webStreamData.DefaultUri = "https://www.google.com";
            }

            var stringfyWebRequest = JsonConvert.SerializeObject(webDataStreamRequest);
            request.Content = new StringContent(stringfyWebRequest);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            HttpResponseMessage response = httpClient.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                WebStreamSucessResponse apiSuceesResponse = JsonConvert.DeserializeObject<WebStreamSucessResponse>(responseBody);

                Console.WriteLine("Web data stream created successfully: " + apiSuceesResponse.WebStreamData.MeasurementId);

                #region  Google Tag Manager Container Creation
                GTMContainer.CreateContainer(apiSuceesResponse.WebStreamData.MeasurementId);
                #endregion

                string measurementId = apiSuceesResponse.WebStreamData.MeasurementId;

                Console.WriteLine("Response: " + responseBody);

                SendGa4Event(apiSuceesResponse.Name, accessToken);
                MarkAsEventConversion(createdGa4Property.Name, accessToken);
            }
            else
            {
                Console.WriteLine("Error Status Code: " + (int)response.StatusCode);
                Console.WriteLine("Error Reason Phrase: " + response.ReasonPhrase);


                string errorResponseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Error Response: " + errorResponseBody);
            }
        }

    }
    /// <summary>
    ///The method named SendGa4Event that uses the Google Analytics 4 (GA4) Measurement Protocol API to create and send events for tracking user interactions. Here's a summary of what the code does:
    ///1.Event Creation:
    ///The method creates two different events: "Click_To_Call" and "Form_Fill_Completion" using the GA4 Measurement Protocol API.
    ///For each event, an event model is created (EventModel instances) with relevant event details like the event name ("Click_To_Call" or "Form_Fill_Completion").
    ///The event model includes information about parameter mutations and event conditions.
    ///2.HTTP Requests:
    ///  The method uses the HttpClient class to send HTTP POST requests to the GA4 Measurement Protocol API to create the events.
    /// For each event, a separate HTTP request is created.
    /// The Authorization header is added to the request with a Bearer token for authentication.
    ///3.Event Sending and Response Handling:
    /// The event model for each event is serialized to JSON format using JsonConvert.SerializeObject.
    ///  The serialized JSON is set as the content of the HTTP request with the appropriate content type.
    ///  The HTTP request is sent asynchronously using httpClient.SendAsync.
    ///  The response from the API is checked for success using IsSuccessStatusCode.
    ///4.Response Processing:
    ///If the API request is successful (HTTP status code 2xx), the response body is read and deserialized to an EventCreateRuleModel object using JsonConvert.DeserializeObject.
    ///  The name of the created event is extracted from the deserialized object and printed to the console.
    /// The entire response body is printed to the console as well.
    ///5.Console Output:
    ///Information about the successful event creation and the API response is printed to the console.
    /// <param name="parentId"></param>
    /// <param name="token"></param>
    /// </summary>
    public static void SendGa4Event(string parentId, string token)
    {
        var events = new List<EventModel>
    {
        new EventModel
        {
            DestinationEvent = "Click_To_Call",
            SourceCopyParameters = false,
            ParameterMutations = new List<ParameterMutation>
            {
                new ParameterMutation { Parameter = "ClickToCall", ParameterValue = "123" }
            },
            EventConditions = new List<EventCondition>
            {
                new EventCondition { ComparisonType = "EQUALS", Field = "Click", Negated = false, Value = "1234" }
            }
        },
        new EventModel
        {
            DestinationEvent = "Form_Fill_Completion",
            SourceCopyParameters = false,
            ParameterMutations = new List<ParameterMutation>
            {
                new ParameterMutation { Parameter = "FormFillCompletion", ParameterValue = "456" }
            },
            EventConditions = new List<EventCondition>
            {
                new EventCondition { ComparisonType = "EQUALS", Field = "FormFill", Negated = false, Value = "5678" }
            }
        }
    };

        using (var httpClient = new HttpClient())
        {
            foreach (var evnt in events)
            {
                var url = $"{baseUrl}{parentId}/eventCreateRules";
                var requestBody = JsonConvert.SerializeObject(evnt);

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + token);
                    request.Content = new StringContent(requestBody);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        EventCreateRuleModel eventCreateRuleModel = JsonConvert.DeserializeObject<EventCreateRuleModel>(responseBody);
                        Console.WriteLine("Event created successfully: " + eventCreateRuleModel.Name);
                        Console.WriteLine("Response: " + responseBody);
                    }
                }
            }
        }
    }

    /// <summary>   
    ///The method named MarkAsEventConversion that utilizes the Google Analytics 4 (GA4) Management API to mark events as conversion events within a specified property. This process involves marking events as conversions for tracking and analysis purposes. Here's a summary of what the code does:
    ///1.Initialization:
    ///The method utilizes the HttpClient class to send HTTP requests to the GA4 Management API.
    ///The API endpoint for managing conversion events is constructed using the provided baseUrl and the propertyId parameter.
    ///2.Event Conversion Marking:
    ///The method marks two events as conversion events: "Click_To_Call" and "Form_Fill_Completion".
    ///For each event, an EventConversion object is created and populated with the event's name ("Click_To_Call" or "Form_Fill_Completion").
    ///3.HTTP Requests:
    ///   The method sends HTTP POST requests to the GA4 Management API to mark the events as conversion events.
    ///Separate HTTP requests are created for each event.
    ///4.Authorization and Content Setup:
    ///The Bearer token (accessToken) is added to the request's Authorization header.
    /// The serialized JSON representation of the EventConversion object is set as the content of the HTTP request.
    ///The content type header is set to indicate that the request body is in JSON format.
    ///5.Response Handling:
    /// The httpClient.SendAsync method is used to send the HTTP requests asynchronously.
    ///The responses from the API are captured in HttpResponseMessage objects (clickToCallResponse and formFillResponse).
    /// <param name="propertyId"></param>
    /// <param name="accessToken"></param>
    /// </summary>
    public static void MarkAsEventConversion(string propertyId, string accessToken)
    {
        using (var httpClient = new HttpClient())
        {
            var conversionEventsUrl = $"{baseUrl}{propertyId}/conversionEvents";

            var eventNames = new List<string> { "Click_To_Call", "Form_Fill_Completion" };

            foreach (var eventName in eventNames)
            {
                var eventConversion = new EventConversion { EventName = eventName };
                var eventJson = JsonConvert.SerializeObject(eventConversion);

                using var request = new HttpRequestMessage(HttpMethod.Post, conversionEventsUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StringContent(eventJson);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = httpClient.SendAsync(request).Result;
                HandleResponse(response, eventName);
            }
        }
    }

    private static void HandleResponse(HttpResponseMessage response, string eventName)
    {
        if (response.IsSuccessStatusCode)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            ConversionEventSuccessModel conversionEventSuccess = JsonConvert.DeserializeObject<ConversionEventSuccessModel>(responseBody);
            Console.WriteLine($"Event Conversion successfully:"+ conversionEventSuccess);
            Console.WriteLine("Response: " + responseBody);
        }
        else
        {
            Console.WriteLine($"Error for event {eventName}: {response.StatusCode}");
        }
    }




}

