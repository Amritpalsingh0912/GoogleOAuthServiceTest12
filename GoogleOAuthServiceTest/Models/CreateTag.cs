using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoogleOAuthServiceTest.Models
{
    public class CreateTag
    {
        [JsonProperty("exportFormatVersion")]
        public long ExportFormatVersion { get; set; }

        [JsonProperty("exportTime")]
        public DateTimeOffset ExportTime { get; set; }

        [JsonProperty("containerVersion")]
        public ContainerVersion ContainerVersion { get; set; }
    }

    public partial class ContainerVersion
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("containerId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long ContainerId { get; set; }

        [JsonProperty("containerVersionId")]
        //  [JsonConverter(typeof(ParseStringConverter))]
        public long ContainerVersionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("container")]
        public Containers Container { get; set; }

        [JsonProperty("tag")]
        public Tags[] Tag { get; set; }

        [JsonProperty("trigger")]
        public Triggers[] Trigger { get; set; }

        [JsonProperty("variable")]
        public Variables[] Variable { get; set; }

        [JsonProperty("folder")]
        public Folder[] Folder { get; set; }

        [JsonProperty("builtInVariable")]
        public BuiltInVariable[] BuiltInVariable { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("tagManagerUrl")]
        public Uri TagManagerUrl { get; set; }
    }

    public partial class BuiltInVariable
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("containerId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long ContainerId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Containers
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("containerId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long ContainerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("publicId")]
        public string PublicId { get; set; }

        [JsonProperty("usageContext")]
        public string[] UsageContext { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("tagManagerUrl")]
        public Uri TagManagerUrl { get; set; }

        [JsonProperty("features")]
        public Dictionary<string, bool> Features { get; set; }

        [JsonProperty("tagIds")]
        public string[] TagIds { get; set; }
    }

    public partial class Folder
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("containerId")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long ContainerId { get; set; }

        [JsonProperty("folderId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long FolderId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }
    }

    public partial class Tags
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("containerId")]
        //  [JsonConverter(typeof(ParseStringConverter))]
        public string ContainerId { get; set; }

        [JsonProperty("tagId")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public string TagId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameter")]
        public Parameter[] Parameters { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("firingTriggerId")]
        // [JsonConverter(typeof(DecodeArrayConverter))]
        public IList<string> FiringTriggerId { get; set; }

        [JsonProperty("parentFolderId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string ParentFolderId { get; set; }

        [JsonProperty("tagFiringOption")]
        public string TagFiringOption { get; set; }

        [JsonProperty("monitoringMetadata")]
        public MonitoringMetadata MonitoringMetadata { get; set; }

        [JsonProperty("consentSettings")]
        public ConsentSettings ConsentSettings { get; set; }
    }

    public partial class ConsentSettings
    {
        [JsonProperty("consentStatus")]
        public string ConsentStatus { get; set; }
    }

    public partial class MonitoringMetadata
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Parameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Triggers
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("containerId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long ContainerId { get; set; }

        [JsonProperty("triggerId")]
        //   [JsonConverter(typeof(ParseStringConverter))]
        public long TriggerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public Filter[] Filter { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("parentFolderId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public long ParentFolderId { get; set; }

        [JsonProperty("waitForTags", NullValueHandling = NullValueHandling.Ignore)]
        public CheckValidation WaitForTags { get; set; }

        [JsonProperty("checkValidation", NullValueHandling = NullValueHandling.Ignore)]
        public CheckValidation CheckValidation { get; set; }

        [JsonProperty("waitForTagsTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public CheckValidation WaitForTagsTimeout { get; set; }

        [JsonProperty("uniqueTriggerId", NullValueHandling = NullValueHandling.Ignore)]
        public MonitoringMetadata UniqueTriggerId { get; set; }
    }

    public partial class CheckValidation
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameter")]
        public Parameter[] Parameter { get; set; }
    }

    public partial class Variables
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("containerId")]
        //  [JsonConverter(typeof(ParseStringConverter))]
        public string ContainerId { get; set; }

        [JsonProperty("variableId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string VariableId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameter")]
        public Parameter[] Parameter { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("parentFolderId")]
        // [JsonConverter(typeof(ParseStringConverter))]
        public string ParentFolderId { get; set; }
    }

    public enum TypeEnum { Boolean, TAG_REFERENCE, Template };

    public partial struct Value
    {
        public bool? Bool;
        public long? Integer;
        public string String; // Add string property

        public static implicit operator Value(bool Bool) => new Value { Bool = Bool };
        public static implicit operator Value(long Integer) => new Value { Integer = Integer };
        public static implicit operator Value(string String) => new Value { String = String }; // Add string conversion
    }
}

