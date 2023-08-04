using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class Competition
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("code")]
        public string Code;

        [JsonPropertyName("type")]
        public string Type;

        [JsonPropertyName("emblem")]
        public string Emblem;
    }
}
