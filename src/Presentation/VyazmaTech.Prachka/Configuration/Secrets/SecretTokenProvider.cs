using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VyazmaTech.Prachka.Presentation.WebAPI.Exceptions;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;

internal static class SecretTokenProvider
{
    internal static async Task<string> GetToken()
    {
        const string host = "http://169.254.169.254/";
        const string uri = "computeMetadata/v1/instance/service-accounts/default/token";

        using var httpClient = new HttpClient { BaseAddress = new Uri(host) };

        using var request = new HttpRequestMessage(HttpMethod.Get, uri)
        {
            Headers =
            {
                { "Metadata-Flavor", "Google" }
            }
        };

        HttpResponseMessage response = await httpClient.SendAsync(request);

        if (response.StatusCode is not HttpStatusCode.OK)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Unable to receive IAM service account token from Yandex Compute Cloud.");
            stringBuilder.AppendLine($"HTTP Status code: {response.StatusCode:D)}");
            string body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body) is false)
            {
                stringBuilder.AppendLine("HTTP Response:");
                stringBuilder.AppendLine(body);
            }

            throw new StartupException(stringBuilder.ToString());
        }

        string content = await response.Content.ReadAsStringAsync();
        JObject? jsonContent = JsonConvert.DeserializeObject<JObject>(content);

        if (jsonContent is null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Unable to parse IAM service account token from Yandex Compute Cloud.");
            stringBuilder.AppendLine("Cannot parse JSON. Original response:");
            stringBuilder.AppendLine(content);

            throw new StartupException(stringBuilder.ToString());
        }

        if (jsonContent.TryGetValue("access_token", StringComparison.Ordinal, out JToken? accessToken))
        {
            return accessToken.ToString();
        }

        var parseStringBuilder = new StringBuilder();
        parseStringBuilder.AppendLine("Unable to parse IAM service account token from Yandex Compute Cloud.");
        parseStringBuilder.AppendLine("Cannot find access token in original response:");
        parseStringBuilder.AppendLine(content);

        throw new StartupException(parseStringBuilder.ToString());
    }
}