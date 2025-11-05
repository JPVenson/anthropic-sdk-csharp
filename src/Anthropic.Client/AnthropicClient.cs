using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Anthropic.Client.Core;
using Anthropic.Client.Exceptions;
using Anthropic.Client.Services.Beta;
using Anthropic.Client.Services.Messages;
using Anthropic.Client.Services.Models;

namespace Anthropic.Client;

public class AnthropicClient : IAnthropicClient
{
    public HttpClient HttpClient { get; init; } = new();

    Lazy<Uri> _baseUrl = new(() =>
        new Uri(
            Environment.GetEnvironmentVariable("ANTHROPIC_BASE_URL") ?? "https://api.anthropic.com"
        )
    );
    public Uri BaseUrl
    {
        get { return _baseUrl.Value; }
        init { _baseUrl = new(() => value); }
    }

    Lazy<string?> _apiKey = new(() => Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY"));
    public string? APIKey
    {
        get { return _apiKey.Value; }
        init { _apiKey = new(() => value); }
    }

    Lazy<string?> _authToken = new(() => Environment.GetEnvironmentVariable("ANTHROPIC_AUTH_TOKEN")
    );
    public string? AuthToken
    {
        get { return _authToken.Value; }
        init { _authToken = new(() => value); }
    }

    readonly Lazy<IMessageService> _messages;
    public IMessageService Messages
    {
        get { return _messages.Value; }
    }

    readonly Lazy<IModelService> _models;
    public IModelService Models
    {
        get { return _models.Value; }
    }

    readonly Lazy<IBetaService> _beta;
    public IBetaService Beta
    {
        get { return _beta.Value; }
    }

    protected virtual ValueTask BeforeSend<T>(HttpRequest<T> request, HttpRequestMessage requestMessage)
        where T : ParamsBase
    {
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask AfterSend<T>(HttpRequest<T> request, HttpResponseMessage httpResponseMessage)
        where T : ParamsBase
    {
        return ValueTask.CompletedTask;
    }

    public async Task<HttpResponse> Execute<T>(HttpRequest<T> request)
        where T : ParamsBase
    {
        using HttpRequestMessage requestMessage = new(request.Method, request.Params.Url(this))
        {
            Content = request.Params.BodyContent(),
        };
        request.Params.AddHeadersToRequest(requestMessage, this);
        HttpResponseMessage responseMessage;
        try
        {
            await BeforeSend(request, requestMessage).ConfigureAwait(false);
            responseMessage = await this
                .HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);
            await AfterSend(request, responseMessage).ConfigureAwait(false);
        }
        catch (HttpRequestException e1)
        {
            throw new AnthropicIOException("I/O exception", e1);
        }
        if (!responseMessage.IsSuccessStatusCode)
        {
            try
            {
                throw AnthropicExceptionFactory.CreateApiException(
                    responseMessage.StatusCode,
                    await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false)
                );
            }
            catch (HttpRequestException e)
            {
                throw new AnthropicIOException("I/O Exception", e);
            }
            finally
            {
                responseMessage.Dispose();
            }
        }
        return new() { Message = responseMessage };
    }

    public AnthropicClient()
    {
        _messages = new(() => new MessageService(this));
        _models = new(() => new ModelService(this));
        _beta = new(() => new BetaService(this));
    }
}
