using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Threem.File.UploaderKit.Interfaces;
using Threem.File.UploaderKit.Models;

namespace Threem.File.UploaderKit.Services
{
    public class SqlMetadataStore : IMetadataStore
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlMetadataStore> _logger;

        public SqlMetadataStore(string connectionString, ILogger<SqlMetadataStore> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task InsertAsync(FileMetadata metadata)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO FileMetadata (FileName, ClientId, BlobUri, UploadDate, FileSize, MimeType, SHA256, AdditionalData) VALUES (@FileName, @ClientId, @BlobUri, @UploadDate, @FileSize, @MimeType, @SHA256 , @AdditionalData)", connection);
               
                command.Parameters.AddWithValue("@FileName", metadata.FileName);
                command.Parameters.AddWithValue("@ClientId", metadata.ClientId);
                command.Parameters.AddWithValue("@BlobUri", metadata.BlobUri);
                command.Parameters.AddWithValue("@UploadDate", metadata.UploadDate);
                command.Parameters.AddWithValue("@FileSize", metadata.FileSize);
                command.Parameters.AddWithValue("@MimeType", metadata.MimeType);
                command.Parameters.AddWithValue("@SHA256", metadata.SHA256);

                string additionalDataJson = JsonSerializer.Serialize(metadata.AdditionalData ?? new Dictionary<string, object>());
                command.Parameters.AddWithValue("@AdditionalData", additionalDataJson);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

}
