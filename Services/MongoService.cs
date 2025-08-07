using MongoDB.Driver;
using Threem.File.UploaderKit.Models;
using Microsoft.Extensions.Logging;
using Threem.File.UploaderKit.Interfaces;

namespace Threem.File.UploaderKit.Services
{
    public class MongoService : IMetadataStore
    {
        private readonly IMongoCollection<FileMetadata> _collection;
        private readonly ILogger<MongoService> _logger;

        public MongoService(string connectionString, string databaseName, string collectionName, ILogger<MongoService> logger)
        {
            _logger = logger;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<FileMetadata>(collectionName);
        }

        public async Task InsertAsync(FileMetadata metadata)
        {
            await _collection.InsertOneAsync(metadata);
        }
    }
}
