using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleOAuthServiceTest.Models
{
    public class WebStreamErrorResponse
    {

        [JsonProperty("@type")]
        public string Type { get; set; }
        public string Reason { get; set; }
        public string Domain { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public List<WebStreamErrorResponse> ApiResponses { get; set; }
    }

    public class Metadata
    {
        public string Service { get; set; }
    }

    public class Root
    {
        public Error Error { get; set; }
    }


}
