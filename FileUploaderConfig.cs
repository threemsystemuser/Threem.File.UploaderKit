namespace Threem.File.UploaderKit
{
    public class MongoConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }

    public class SqlConfig
    {
        public string ConnectionString { get; set; }
    }
    public enum MetadataStoreType
    {
        MongoDB,
        SqlServer,


    }


    public class FileUploaderSettings
    {
        public string AzureBlobConnectionString { get; set; }
        public string AzureBlobContainer { get; set; }
        public string LogDirectory { get; set; }
        public MetadataStoreType StoreType { get; set; }
        public MongoConfig Mongo { get; set; }
        public SqlConfig Sql { get; set; }
    }
}
