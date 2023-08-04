using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class HalfTime
    {
        [JsonPropertyName("home")]
        public object Home;

        [JsonPropertyName("away")]
        public object Away;
    }
}
