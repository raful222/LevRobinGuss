using System.Text.Json.Serialization;

namespace LevRobinGuss.Models.FootballApiModel
{
    public class Filters
    {
        [JsonPropertyName("dateFrom")]
        public string DateFrom;

        [JsonPropertyName("dateTo")]
        public string DateTo;

        [JsonPropertyName("permission")]
        public string Permission;
    }
}
