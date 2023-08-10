using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleOAuthServiceTest.Models
{
    public class EventCreateRuleModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("destinationEvent")]
        public string DestinationEvent { get; set; }
        [JsonProperty("eventConditions")]
        public List<Event_Condition> EventConditions { get; set; }
        [JsonProperty("parameterMutations")]
        public List<Parameter_Mutation> ParameterMutations { get; set; }
    }

    public class Event_Condition
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("comparisonType")]
        public string ComparisonType { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Parameter_Mutation
    {
        [JsonProperty("parameter")]
        public string Parameter { get; set; }
        [JsonProperty("parameterValue")]
        public string ParameterValue { get; set; }
    }
}
