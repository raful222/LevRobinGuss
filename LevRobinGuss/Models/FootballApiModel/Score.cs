using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class Score
    {
        [JsonPropertyName("winner")]
        public object Winner;

        [JsonPropertyName("duration")]
        public string Duration;

        [JsonPropertyName("fullTime")]
        public FullTime FullTime;

        [JsonPropertyName("halfTime")]
        public HalfTime HalfTime;
    }

}
