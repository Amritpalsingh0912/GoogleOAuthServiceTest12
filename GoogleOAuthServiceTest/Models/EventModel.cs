using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleOAuthServiceTest.Models
{
    public class EventModel
    {
        [JsonProperty("destinationEvent")]
        public string DestinationEvent { get; set; }
        [JsonProperty("sourceCopyParameters")]
        public bool SourceCopyParameters { get; set; }
        [JsonProperty("parameterMutations")]
        public List<ParameterMutation> ParameterMutations { get; set; }
        [JsonProperty("eventConditions")]
        public List<EventCondition> EventConditions { get; set; }
    }

    public class ParameterMutation
    {
        [JsonProperty("parameter")]
        public string Parameter { get; set; }
        [JsonProperty("parameterValue")]
        public string ParameterValue { get; set; }
    }

    public class EventCondition
    {
        [JsonProperty("comparisonType")]
        public string ComparisonType { get; set; }
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("negated")]
        public bool Negated { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
