using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class HomeTeam
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("shortName")]
        public string ShortName;

        [JsonPropertyName("tla")]
        public string Tla;

        [JsonPropertyName("crest")]
        public string Crest;
    }
}
