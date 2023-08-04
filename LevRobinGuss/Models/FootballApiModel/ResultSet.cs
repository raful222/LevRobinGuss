using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class ResultSet
    {
        [JsonPropertyName("count")]
        public int Count;

        [JsonPropertyName("competitions")]
        public string Competitions;

        [JsonPropertyName("first")]
        public string First;

        [JsonPropertyName("last")]
        public string Last;

        [JsonPropertyName("played")]
        public int Played;
    }
}
