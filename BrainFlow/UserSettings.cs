using Newtonsoft.Json;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    public class UserSettings
    {
        [JsonProperty(PropertyName = "board")]
        public string Board { get; set; }

        [JsonProperty(PropertyName = "boardId")]
        public int BoardId { get; set; }

        [JsonProperty(PropertyName = "ipAddress")]
        public string IpAddress { get; set; }

        [JsonProperty(PropertyName = "ipPort")]
        public int IpPort { get; set; }

        [JsonProperty(PropertyName = "ipProtocol")]
        public int IpProtocol { get; set; }

        [JsonProperty(PropertyName = "macAddress")]
        public string MacAddress { get; set; }

        [JsonProperty(PropertyName = "serialPort")]
        public string SerialPort { get; set; }

        [JsonProperty(PropertyName = "serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "timeout")]
        public int Timeout { get; set; }

        [JsonProperty(PropertyName = "otherInfo")]
        public string OtherInfo { get; set; }

        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }
    }
}
