using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class FullTime
    {
        [JsonPropertyName("home")]
        public object Home;

        [JsonPropertyName("away")]
        public object Away;
    }
}
