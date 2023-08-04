using LevRobinGuss.Models.FootballApiModel;
using Newtonsoft.Json;
using System.Text.Json;

namespace LevRobinGuss.Service
{
    public class FootballDataAPi
    {
        public static readonly string BASE_URL = "https://api.football-data.org/";
        public static readonly string API_KEY = "be27c8defa6145138cf59ead5c8b807b";
        public async Task<GameMatches> MatchesList(string toDate, string fromDate)
        {
            var uriBuilder = new UriBuilder(BASE_URL);
            uriBuilder.Path = "v4/matches";
            uriBuilder.Query = $"dateFrom={fromDate}&dateTo={toDate}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Auth-Token", API_KEY);
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri))
                {
                    requestMessage.Headers.Add("X-Unfold-Goals", "true");

                    try
                    {
                        var response = client.SendAsync(requestMessage).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResult = await response.Content.ReadAsStringAsync();

                            var result = JsonConvert.DeserializeObject<GameMatches>(jsonResult,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    Error = (sender, errorArgs) =>
                                    {
                                        errorArgs.ErrorContext.Handled = true;
                                    }
                                });

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        //_logger.LogError(ex, ex.Message);
                    }
                }
            }

            return null;
        }

    }
}
