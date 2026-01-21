using Google.Apis.Auth.OAuth2;

namespace Anthropic.Vertex;

/// <summary>
/// Defines methods to authenticate with vertex services using the <see cref="GoogleCredential"/> api.
/// </summary>
public class AnthropicVertexCredentials : IAnthropicVertexCredentials
{
    private readonly GoogleCredential _googleCredentials;
    private readonly string _audienceUrl;
    private OidcToken? _token;

    /// <summary>
    /// Creates a new instance of the <see cref="AnthropicVertexCredentials"/> using the environment provided google authentication methods.
    /// </summary>
    /// <param name="region">The region string for the project or <c>null</c> for global.</param>
    /// <param name="project">The project string.</param>
    /// <param name="audienceUrl">The OIDC audience url.</param>
    public AnthropicVertexCredentials(string? region, string project, string audienceUrl)
        : this(region, project, audienceUrl, GoogleCredential.GetApplicationDefault()) { }

    /// <summary>
    /// Creates a new instance of the <see cref="AnthropicVertexCredentials"/>.
    /// </summary>
    /// <param name="region">The region string for the project or <c>null</c> for global.</param>
    /// <param name="project">The project string.</param>
    /// <param name="audienceUrl">The OIDC audience url.</param>
    /// <param name="googleCredential">The authentication method.</param>
    public AnthropicVertexCredentials(
        string? region,
        string project,
        string audienceUrl,
        GoogleCredential googleCredential
    )
    {
        Region = region;
        Project = project;
        _audienceUrl = audienceUrl;
        _googleCredentials = googleCredential;
    }

    /// <inheritdoc/>
    public string? Region { get; }

    /// <inheritdoc/>
    public string Project { get; }

    /// <inheritdoc/>
    public async ValueTask ApplyAsync(HttpRequestMessage requestMessage)
    {
        _token ??= await _googleCredentials
            .GetOidcTokenAsync(OidcTokenOptions.FromTargetAudience(_audienceUrl))
            .ConfigureAwait(false);
        var bearerToken = await _token.GetAccessTokenAsync();
        requestMessage.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("bearer " + bearerToken);
    }
}
