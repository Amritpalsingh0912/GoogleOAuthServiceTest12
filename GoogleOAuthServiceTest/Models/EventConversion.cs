using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleOAuthServiceTest.Models
{
    public class EventConversion
    {
        [JsonProperty("eventName")]
        public string EventName { get; set; }
    }
}
