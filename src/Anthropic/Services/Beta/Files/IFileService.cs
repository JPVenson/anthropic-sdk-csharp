using System.Threading.Tasks;
using Anthropic.Models.Beta.Files;

namespace Anthropic.Services.Beta.Files;

public interface IFileService
{
    /// <summary>
    /// List Files
    /// </summary>
    Task<FileListPageResponse> List(FileListParams parameters);

    /// <summary>
    /// Delete File
    /// </summary>
    Task<DeletedFile> Delete(FileDeleteParams parameters);

    /// <summary>
    /// Get File Metadata
    /// </summary>
    Task<FileMetadata> RetrieveMetadata(FileRetrieveMetadataParams parameters);
}
