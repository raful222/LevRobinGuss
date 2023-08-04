using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class Area
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("code")]
        public string Code;

        [JsonPropertyName("flag")]
        public string Flag;
    }
}
