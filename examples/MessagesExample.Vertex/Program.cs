using Anthropic.Models.Messages;
using Anthropic.Vertex;
using Google.Apis.Auth.OAuth2;

// The google vertex client needs a Project ID, use the ID from the google cloud dashboard.
// The region parameter is optional.

// By default the Vertex Credential provider tries to load system wide credentials generated via the "gcloud" tool.
// For application wide credentials we recommend using service accounts instead and providing your own GoogleCredentials. Example:
/*
var client = new AnthropicVertexClient(new AnthropicVertexCredentials(null, "YourProjectId", GoogleCredential.FromJson(
"""
{
    ServiceAccount JSON
}
""").CreateScoped("https://www.googleapis.com/auth/cloud-platform")));
*/

var client = new AnthropicVertexClient(new AnthropicVertexCredentials(null, "YourProjectId"));

MessageCreateParams parameters = new()
{
    MaxTokens = 2048,
    Messages =
    [
        new() { Content = "Tell me a story about building the best SDK!", Role = Role.User },
    ],
    Model = "claude-sonnet-4-5",
};

var response = await client.Messages.Create(parameters);

var message = string.Join(
    "",
    response
        .Content.Where(message => message.Value is TextBlock)
        .Select(message => message.Value as TextBlock)
        .Select((textBlock) => textBlock.Text)
);

Console.WriteLine(message);
