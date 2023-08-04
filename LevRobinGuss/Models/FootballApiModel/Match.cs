using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class Match
    {
        [JsonPropertyName("area")]
        public Area Area;

        [JsonPropertyName("competition")]
        public Competition Competition;

        [JsonPropertyName("season")]
        public Season Season;

        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("utcDate")]
        public DateTime UtcDate;

        [JsonPropertyName("status")]
        public string Status;

        [JsonPropertyName("matchday")]
        public int Matchday;

        [JsonPropertyName("stage")]
        public string Stage;

        [JsonPropertyName("group")]
        public object Group;

        [JsonPropertyName("lastUpdated")]
        public DateTime LastUpdated;

        [JsonPropertyName("homeTeam")]
        public HomeTeam HomeTeam;

        [JsonPropertyName("awayTeam")]
        public AwayTeam AwayTeam;

        [JsonPropertyName("score")]
        public Score Score;

        [JsonPropertyName("odds")]
        public Odds Odds;

        [JsonPropertyName("referees")]
        public List<object> Referees;
    }
}
