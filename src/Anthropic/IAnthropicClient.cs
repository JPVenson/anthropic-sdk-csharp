using System;
using System.Net.Http;
using Anthropic.Services.Beta;
using Anthropic.Services.Messages;
using Anthropic.Services.Models;

namespace Anthropic;

public interface IAnthropicClient
{
    HttpClient HttpClient { get; init; }

    Uri BaseUrl { get; init; }

    string? APIKey { get; init; }

    string? AuthToken { get; init; }

    IMessageService Messages { get; }

    IModelService Models { get; }

    IBetaService Beta { get; }
}
