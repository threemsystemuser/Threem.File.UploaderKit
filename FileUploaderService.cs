using Microsoft.Extensions.Logging;
using Threem.File.UploaderKit.Interfaces;
using Threem.File.UploaderKit.Models;
using Threem.File.UploaderKit.Utils;
using System.Security.Cryptography;
using NLog.Extensions.Logging;
using Threem.File.UploaderKit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Threem.File.UploaderKit
{
    public class FileUploaderService : IFileUploader
    {
        private readonly IMetadataStore _metadataStore;
        private readonly IAzureBlobService _blobService;
        private readonly ILogger<FileUploaderService> _logger;

        public FileUploaderService( FileUploaderSettings settings)
        {
        
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog(NLogHelper.ConfigureNLog(settings.LogDirectory));
            });

            _logger = loggerFactory.CreateLogger<FileUploaderService>();

            _blobService = new AzureBlobService(
                settings.AzureBlobConnectionString,
                settings.AzureBlobContainer,
                loggerFactory.CreateLogger<AzureBlobService>());

            _metadataStore = settings.StoreType switch
            {
                MetadataStoreType.MongoDB => new MongoService(
                    settings.Mongo.ConnectionString,
                    settings.Mongo.Database,
                    settings.Mongo.Collection,
                    loggerFactory.CreateLogger<MongoService>()),

                MetadataStoreType.SqlServer => new SqlMetadataStore(
                    settings.Sql.ConnectionString,
                    loggerFactory.CreateLogger<SqlMetadataStore>()),

                _ => throw new ArgumentException("Unsupported metadata store type.")
            };

            GlobalExceptionHandler.RegisterGlobalHandlers(_logger);
        }

        public async Task UploadAsync(IFormFile file, string clientId, Dictionary<string, object>? additionalData = null)
        {
            _logger.LogInformation("Upload started for file: {FileName}, clientId: {ClientId}", file.FileName, clientId);

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();

            _logger.LogInformation("Read {ByteCount} bytes from uploaded file: {FileName}", bytes.Length, file.FileName);

            string fileName = file.FileName;
            _logger.LogInformation("Extracted file name: {FileName}", fileName);

            memoryStream.Position = 0; // Reset stream position before reusing it
            _logger.LogInformation("Uploading file to Azure Blob Storage...");

            string uri = await _blobService.UploadFileAsync(memoryStream, fileName);
            _logger.LogInformation("File uploaded. Blob URI: {BlobUri}", uri);

            string sha256;
            using (var sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(bytes);
                sha256 = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
            _logger.LogInformation("SHA256 hash calculated: {SHA256}", sha256);

            var metadata = new FileMetadata
            {
                FileName = fileName,
                ClientId = clientId,
                BlobUri = uri,
                UploadDate = DateTime.UtcNow,
                FileSize = bytes.Length,
                MimeType = MimeTypes.GetMimeType(fileName),
                SHA256 = sha256,
                AdditionalData = new Dictionary<string, object>()
            };

            if (additionalData != null)
            {
                foreach (var kvp in additionalData)
                {
                    metadata.AdditionalData[kvp.Key] = kvp.Value;
                }
            }

            _logger.LogInformation("Inserting metadata for file: {FileName}", fileName);
            await _metadataStore.InsertAsync(metadata);
            _logger.LogInformation("Metadata inserted successfully for file: {FileName}", fileName);

            _logger.LogInformation("Upload process completed for file: {FileName}", fileName);
        }
        


    }
}