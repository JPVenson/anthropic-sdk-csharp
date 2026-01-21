namespace Anthropic.Vertex;

/// <summary>
/// Defines methods for authenticating requests to the vertex api.
/// </summary>
public interface IAnthropicVertexCredentials
{
    /// <summary>
    /// Gets the Region on the Project.
    /// </summary>
    string? Region { get; }

    /// <summary>
    /// Gets the Project name.
    /// </summary>
    string Project { get; }

    /// <summary>
    /// Applies the authentication method to the request.
    /// </summary>
    /// <param name="requestMessage">The http Request message object.</param>
    /// <returns>A value task that is resolved when the authentication has been applied to the request message.</returns>
    ValueTask ApplyAsync(HttpRequestMessage requestMessage);
}
