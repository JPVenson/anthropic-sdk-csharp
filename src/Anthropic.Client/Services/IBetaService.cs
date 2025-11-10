using System;
using Anthropic.Client.Core;
using Beta = Anthropic.Client.Services.Beta;

namespace Anthropic.Client.Services;

public interface IBetaService
{
    IBetaService WithOptions(Func<ClientOptions, ClientOptions> modifier);

    Beta::IModelService Models { get; }

    Beta::IMessageService Messages { get; }

    Beta::IFileService Files { get; }

    Beta::ISkillService Skills { get; }
}
