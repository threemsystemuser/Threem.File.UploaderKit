using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Threem.File.UploaderKit.Models
{
    public class FileMetadata
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FileName { get; set; }
        public string ClientId { get; set; }
        public string BlobUri { get; set; }
        public DateTime UploadDate { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
        public string SHA256 { get; set; }

        [BsonExtraElements]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

    }
}
