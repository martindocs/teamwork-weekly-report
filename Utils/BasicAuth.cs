
using System.Net.Http.Headers;

namespace TeamworkWeeklyReport.Utils
{
    public static class BasicAuth
    {

        public static void SetTeamworkBasicAuth(string teamworkPassword, string apiToken, HttpClient httpClient){
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes($"{apiToken}:{teamworkPassword}");
            var tokenBase64 = Convert.ToBase64String(tokenBytes);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", tokenBase64);
        }
    }
}
