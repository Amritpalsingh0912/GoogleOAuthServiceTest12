using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleOAuthServiceTest.Models
{
    public class WebDataStreamRequest
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

        public WebRequestStreamData webStreamData { get; set; }
    }
    public class WebRequestStreamData
    {
        [JsonProperty("defaultUri")]
        public string DefaultUri { get; set; }
    }
}
