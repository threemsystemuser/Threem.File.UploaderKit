using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Threem.File.UploaderKit.Interfaces;

namespace Threem.File.UploaderKit.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(string connectionString, string containerName, ILogger<AzureBlobService> logger)
        {
            _logger = logger;
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            _logger.LogInformation("the filename is" + fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
            return blobClient.Uri.ToString();
        }
    }
}
