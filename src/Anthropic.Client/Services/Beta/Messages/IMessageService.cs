using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anthropic.Client.Core;
using Anthropic.Client.Services.Beta.Messages.Batches;
using Messages = Anthropic.Client.Models.Beta.Messages;

namespace Anthropic.Client.Services.Beta.Messages;

public interface IMessageService
{
    IMessageService WithOptions(Func<ClientOptions, ClientOptions> modifier);

    IBatchService Batches { get; }

    /// <summary>
    /// Send a structured list of input messages with text and/or image content,
    /// and the model will generate the next message in the conversation.
    ///
    /// <para>The Messages API can be used for either single queries or stateless
    /// multi-turn conversations.</para>
    ///
    /// <para>Learn more about the Messages API in our [user guide](/en/docs/initial-setup)</para>
    /// </summary>
    Task<Messages::BetaMessage> Create(
        Messages::MessageCreateParams parameters,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Send a structured list of input messages with text and/or image content,
    /// and the model will generate the next message in the conversation.
    ///
    /// <para>The Messages API can be used for either single queries or stateless
    /// multi-turn conversations.</para>
    ///
    /// <para>Learn more about the Messages API in our [user guide](/en/docs/initial-setup)</para>
    /// </summary>
    IAsyncEnumerable<Messages::BetaRawMessageStreamEvent> CreateStreaming(
        Messages::MessageCreateParams parameters,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Count the number of tokens in a Message.
    ///
    /// <para>The Token Count API can be used to count the number of tokens in a Message,
    /// including tools, images, and documents, without creating it.</para>
    ///
    /// <para>Learn more about token counting in our [user guide](/en/docs/build-with-claude/token-counting)</para>
    /// </summary>
    Task<Messages::BetaMessageTokensCount> CountTokens(
        Messages::MessageCountTokensParams parameters,
        CancellationToken cancellationToken = default
    );
}
