using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class Season
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("startDate")]
        public string StartDate;

        [JsonPropertyName("endDate")]
        public string EndDate;

        [JsonPropertyName("currentMatchday")]
        public int CurrentMatchday;

        [JsonPropertyName("winner")]
        public object Winner;
    }

}
