using Newtonsoft.Json;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    internal class Board
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
