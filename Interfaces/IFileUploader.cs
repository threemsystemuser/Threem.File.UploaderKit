using Microsoft.AspNetCore.Http;

namespace Threem.File.UploaderKit.Interfaces
{
    public interface IFileUploader
    {
        Task UploadAsync(IFormFile filePath, string clientId , Dictionary<string, object>? additionalData );
    }

}
