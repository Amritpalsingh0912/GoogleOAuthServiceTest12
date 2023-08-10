using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.TagManager.v2;
using Google.Apis.TagManager.v2.Data;
using GoogleOAuthServiceTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoogleOAuthServiceTest
{
    public class GTMContainer
    {
        private const string ACCOUNT_ID = "accounts/4701974616";

        public static void CreateContainer(string measurementId)
        {
            try
            {
                var credential = GoogleCredential.FromFile("googleauth.json").CreateScoped(
                    new[] { "https://www.googleapis.com/auth/tagmanager.edit.containers" });
                var service = new TagManagerService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });

                Console.Write("Enter GTM Container Name: ");
                string gtmContainerName = Console.ReadLine();
                Console.Write("Enter hostName: ");
                string hostName = Console.ReadLine();

                // Create the GTM container
                var container = new Container
                {
                    Name = gtmContainerName,
                    UsageContext = new[] { "web" },
                    DomainName = new List<string>() { hostName }
                };
                var createContainerRequest = service.Accounts.Containers.Create(container, ACCOUNT_ID);
                var createdContainer = createContainerRequest.Execute();
                Console.WriteLine("GTM container created successfully: " + createdContainer.ContainerId);

                // Create a workspace for the container
                var workspace = new Workspace
                {
                    Name = "YourWorkspace" // Replace with your desired workspace name
                };
                var createWorkspaceRequest = service.Accounts.Containers.Workspaces.Create(workspace, $"{ACCOUNT_ID}/containers/{createdContainer.ContainerId}");
                var createdWorkspace = createWorkspaceRequest.Execute();
                Console.WriteLine("Workspace created successfully: " + createdWorkspace.Name);

                string currentDirectory = Directory.GetCurrentDirectory();
                string jsonFilePath = Path.Combine(currentDirectory, "HLM-GTM-Container.json");
                string tagsJson = File.ReadAllText(jsonFilePath);

                var rootObject = JsonConvert.DeserializeObject<CreateTag>(tagsJson);
                var tags = rootObject.ContainerVersion.Tag.Where(tag => tag.Type == "gaawe" || tag.Type == "gaawc");

                foreach (var tag in tags)
                {
                    var createTagObject = new Tag
                    {
                        AccountId = tag.AccountId,
                        ContainerId = tag.ContainerId,
                        TagId = tag.TagId,
                        Name = tag.Name,
                        Type = tag.Type,
                        Parameter = tag.Parameters.Select(tagparameter => new Google.Apis.TagManager.v2.Data.Parameter
                        {
                            Key = tagparameter.Key,
                            Value = tagparameter.Key != "measurementId" ? tagparameter.Value : measurementId,
                            Type = tagparameter.Type
                        }).ToList(),
                        Fingerprint = tag.Fingerprint,
                        TagFiringOption = tag.TagFiringOption,
                        MonitoringMetadata = new Google.Apis.TagManager.v2.Data.Parameter
                        {
                            Type = tag.MonitoringMetadata.Type?.ToString()
                        },
                        ConsentSettings = new Google.Apis.TagManager.v2.Data.TagConsentSetting
                        {
                            ConsentStatus = tag.ConsentSettings.ConsentStatus?.ToString()
                        }
                    };

                    var createTagRequest = service.Accounts.Containers.Workspaces.Tags.Create(createTagObject, $"{ACCOUNT_ID}/containers/{createdContainer.ContainerId}/workspaces/{createdWorkspace.WorkspaceId}");
                    var createdTag = createTagRequest.Execute();
                    Console.WriteLine("Tag created successfully: " + createdTag.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            //CreateVariables(createdContainer.ContainerId,service, tagsJson, createdWorkspace.WorkspaceId);
        }

        //public static void CreateVariables(string containerId,TagManagerService service,string json,string workspaceId)
        //{
        //    CreateTag rootObject = JsonConvert.DeserializeObject<CreateTag>(json);
        //    List<Variables> getVariables = rootObject.ContainerVersion.Variable.ToList();
        //    var createTagObject = new Google.Apis.TagManager.v2.Data.Parameter();
        //    try
        //    {
        //        var  newVariable = new Variable();
        //        // Create a new variable object
        //        foreach (var item in getVariables)
        //        {
        //            newVariable.AccountId= item.AccountId;
        //            newVariable.Name= item.Name;
        //            newVariable.ContainerId = item.ContainerId;
        //            newVariable.Type= item.Type;
        //            newVariable.VariableId= item.VariableId;
        //            newVariable.Fingerprint= item.Fingerprint;
        //            newVariable.ParentFolderId = item.ParentFolderId;
        //            //newVariable.AccountId
        //            //newVariable = new Variable
        //            //{
        //            //    a
        //            //    Name = item.Name,
        //            //    Type = item.Type,
        //            //    Notes = "This is a new custom JavaScript variable.",

        //            //    //Parameter = new Google.Apis.TagManager.v2.Data.Parameter { }
        //            //    //Parameter = new Parameter{                    {
        //            //    //    Type = "template",
        //            //    //    Key = "javascript",
        //            //    //    Value = "<your_custom_js_code_here>", 
        //            //    //}
        //            //};
        //            foreach (var getParams in item.Parameter)
        //            {
        //                if (newVariable.Parameter == null)
        //                {
        //                    newVariable.Parameter = new List<Google.Apis.TagManager.v2.Data.Parameter>();
        //                }
        //                newVariable.Parameter.Add(new Google.Apis.TagManager.v2.Data.Parameter()
        //                {
        //                    Key = getParams.Key,
        //                    Value = getParams.Value,
        //                    Type = getParams.Type

        //                }); 
        //            }
        //        }

        //        // Insert the variable into the specified container
        //        var createdVariable = service.Accounts.Containers.Workspaces.Variables.Create(newVariable, "accounts/4701974616/containers/" + containerId + "/workspaces/"+ workspaceId)
        //            .Execute();

        //        Console.WriteLine("Variable created successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("An error occurred: " + ex.Message);
        //    }
        //}
    }
}

