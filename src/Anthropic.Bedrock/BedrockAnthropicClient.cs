using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Anthropic.Client;
using Anthropic.Client.Core;
using Anthropic.Client.Exceptions;

namespace Anthropic.Bedrock;

public class BedrockAnthropicClient : AnthropicClient
{
    private const string ServiceName = "bedrock-runtime";
    private const string AnthropicVersion = "bedrock-2023-05-31";
    private const string HEADER_ANTHROPIC_BETA = "anthropic-beta";

    /// <summary>
    /// The name of the header that identifies the content type for the "payloads" of AWS
    /// _EventStream_ messages in streaming responses from Bedrock.
    /// </summary>
    private const string HEADER_PAYLOAD_CONTENT_TYPE = "x-amzn-bedrock-content-type";

    /// <summary>
    /// The content type for Bedrock responses containing data in the AWS _EventStream_ format.
    /// The value of the[HEADER_PAYLOAD_CONTENT_TYPE] header identifies the content type of the
    /// "payloads" in this stream.
    /// </summary>
    private const string CONTENT_TYPE_AWS_EVENT_STREAM = "application/vnd.amazon.eventstream";

    /// <summary>
    /// The content type for Anthropic responses containing Bedrock data after it has been
    /// translated into the Server-Sent Events (SSE) stream format.
    /// </summary>
    private const string CONTENT_TYPE_SSE_STREAM = "text/event-stream; charset=utf-8";

    private readonly IBedrockCredentials _bedrockCredentials;

    public BedrockAnthropicClient(IBedrockCredentials bedrockCredentials, string region)
    {
        _bedrockCredentials = bedrockCredentials;
        BaseUrl = new Uri($"https://{ServiceName}.{region}.amazonaws.com");
    }

    protected override async ValueTask BeforeSend<T>(HttpRequest<T> request, HttpRequestMessage requestMessage)
    {
        ValidateRequest(requestMessage);

        requestMessage.Headers.TryAddWithoutValidation("anthropic_version", AnthropicVersion);

        var betaVersions = requestMessage.Headers.GetValues(HEADER_ANTHROPIC_BETA).Distinct().ToArray();
        if (betaVersions is not { Length: 0 })
        {
            //TODO BETA REPLACEMENT
        }

        var bodyContent = JsonNode.Parse(await requestMessage.Content!.ReadAsStringAsync().ConfigureAwait(false));

        if (bodyContent["model"] == null)
        {
            throw new AnthropicInvalidDataException("Expected to find property model in request json but found none.");
        }
        var modelValue = bodyContent["model"];
        bodyContent["model"] = null;
        var parsedStreamValue = ((bool?)bodyContent["stream"]?.AsValue()) ?? false;

        var contentStream = new MemoryStream();
        requestMessage.Content = new StreamContent(contentStream);
        using var writer = new Utf8JsonWriter(contentStream);
        {
            bodyContent.WriteTo(writer);
        }

        var uriBuilder = new UriBuilder(requestMessage.RequestUri);
        uriBuilder.Path = string.Join('/', [.. uriBuilder.Path.Split("/").Select(e => e == "model" ? modelValue.ToString() : e), (parsedStreamValue ? "invoke-with-response-stream" : "invoke")]);

        requestMessage.RequestUri = uriBuilder.Uri;
        requestMessage.Headers.TryAddWithoutValidation("Host", uriBuilder.Uri.Host);      

        _bedrockCredentials.Apply(requestMessage);
    }

    private static void ValidateRequest(HttpRequestMessage requestMessage)
    {
        if (requestMessage.RequestUri is null)
        {
            throw new AnthropicInvalidDataException("Request is missing required path segments. Expected > 1 segments found none.");
        }

        if (requestMessage.RequestUri.Segments.Length < 1)
        {
            throw new AnthropicInvalidDataException("Request is missing required path segments. Expected > 1 segments found none.");
        }

        if (requestMessage.RequestUri.Segments[0] != "v1")
        {
            throw new AnthropicInvalidDataException($"Request is missing required path segments. Expected [0] segment to be 'v1' found {requestMessage.RequestUri.Segments[0]}.");
        }

        if (requestMessage.RequestUri.Segments[1] is "messages" && requestMessage.RequestUri.Segments[2] is "batches" or "count_tokens")
        {
            throw new AnthropicInvalidDataException($"The requested endpoint '{requestMessage.RequestUri.Segments[2]}' is not yet supported.");
        }
    }

    protected override async ValueTask AfterSend<T>(HttpRequest<T> request, HttpResponseMessage httpResponseMessage)
    {
        if (!httpResponseMessage.Headers.GetValues("content-type").Any(f => string.Equals(f, CONTENT_TYPE_AWS_EVENT_STREAM, StringComparison.CurrentCultureIgnoreCase)))
        {
            return;
        }

        var headerPayloads = httpResponseMessage.Headers.GetValues(HEADER_PAYLOAD_CONTENT_TYPE);

        if(!headerPayloads.Any(f => f.Equals("application/json", StringComparison.OrdinalIgnoreCase)))
        {
            throw new AnthropicInvalidDataException($"Expected streaming bedrock events to have content type of application/json but found {string.Join(", ", headerPayloads)}");
        }
    }
}
