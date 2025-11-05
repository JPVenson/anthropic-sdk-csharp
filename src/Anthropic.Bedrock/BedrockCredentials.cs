namespace Anthropic.Bedrock;

public class BedrockCredentials : IBedrockCredentials
{
    private BedrockCredentials()
    {

    }

    public string? BearerToken { get; private set; }

    public string? Region { get; private set; }

    public static BedrockCredentials FromApiKey(string bearerToken, string? region = null)
    {
        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            throw new ArgumentNullException(nameof(bearerToken), "The bearer token cannot be null or empty");
        }

        return new()
        {
            BearerToken = bearerToken,
            Region = region
        };
    }

    public void Apply(HttpRequestMessage requestMessage)
    {
        requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", BearerToken);
    }
}
