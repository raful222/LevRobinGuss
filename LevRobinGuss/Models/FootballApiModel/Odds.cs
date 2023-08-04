using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class Odds
    {
        [JsonPropertyName("msg")]
        public string Msg;
    }
}
