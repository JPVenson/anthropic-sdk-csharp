using Anthropic.Client;

namespace Anthropic.Bedrock;

public static class BedrockAnthropicExtensions
{
    public static IAnthropicClient CreateBedrockClient(IBedrockCredentials bedrockCredentials, string region)
    {
        return new BedrockAnthropicClient(bedrockCredentials, region);
    }
}
