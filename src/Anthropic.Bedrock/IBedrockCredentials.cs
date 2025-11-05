namespace Anthropic.Bedrock;

public interface IBedrockCredentials
{
    public void Apply(HttpRequestMessage httpClient);
}
