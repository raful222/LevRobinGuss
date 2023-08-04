using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
        public class GameMatches
        {
            [JsonPropertyName("filters")]
            public Filters Filters;

            [JsonPropertyName("resultSet")]
            public ResultSet ResultSet;

            [JsonPropertyName("matches")]
            public List<Match> Matches;
        }
}
