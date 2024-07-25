using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VyazmaTech.Prachka.Presentation.WebAPI.Exceptions;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;

internal sealed class SecretProvider
{
    private readonly string _token;

    public SecretProvider(string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

        _token = token;
    }

    internal async Task<SecretEntry[]> GetEntries(string secretId)
    {
        ArgumentException.ThrowIfNullOrEmpty(secretId, nameof(secretId));

        const string baseUrl = "https://payload.lockbox.api.cloud.yandex.net/";
        string requestUri = $"/lockbox/v1/secrets/{secretId}/payload";

        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl),
            DefaultRequestHeaders =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", _token),
            },
        };

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage resp = await httpClient.SendAsync(request);

        if (resp.StatusCode is not HttpStatusCode.OK)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Unable to receive secrets from Yandex LockBox.");
            stringBuilder.AppendLine($"HTTP Status code: {resp.StatusCode:D)}");

            string body = await resp.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body) is false)
            {
                stringBuilder.AppendLine("HTTP Response:");
                stringBuilder.AppendLine(body);
            }

            throw new StartupException(stringBuilder.ToString());
        }

        string respBody = await resp.Content.ReadAsStringAsync();
        try
        {
            SecretEntry[]? entries = JsonConvert.DeserializeObject<JObject>(respBody)
                ?.GetValue("entries", StringComparison.Ordinal)
                ?.ToObject<SecretEntry[]>();

            if (entries?.Any() is not true)
            {
                throw new StartupException("Secrets cannot be null or empty");
            }

            return entries;
        }
        catch
        {
            throw new StartupException("Secret manager have not returned any secrets");
        }
    }
}