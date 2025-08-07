using Microsoft.AspNetCore.StaticFiles;

namespace Threem.File.UploaderKit.Utils
{
    /// <summary>
    /// Utility class to determine MIME types based on file extensions.
    /// </summary>
    public static class MimeTypes
    {
        private static readonly FileExtensionContentTypeProvider _provider;
        private static readonly Dictionary<string, string> _mimeCache = new();

        static MimeTypes()
        {
            _provider = new FileExtensionContentTypeProvider();

            // Add additional mappings for file types that may not be covered by default
            _provider.Mappings.TryAdd(".json", "application/json");
            _provider.Mappings.TryAdd(".webp", "image/webp");
            _provider.Mappings.TryAdd(".avif", "image/avif");
            _provider.Mappings.TryAdd(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            _provider.Mappings.TryAdd(".csv", "text/csv");
            _provider.Mappings.TryAdd(".ppt", "application/vnd.ms-powerpoint");
            _provider.Mappings.TryAdd(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            _provider.Mappings.TryAdd(".doc", "application/msword");
            _provider.Mappings.TryAdd(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            _provider.Mappings.TryAdd(".xls", "application/vnd.ms-excel");
            _provider.Mappings.TryAdd(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            _provider.Mappings.TryAdd(".csv", "text/csv");
            _provider.Mappings.TryAdd(".json", "application/json");
            _provider.Mappings.TryAdd(".zip", "application/zip");
            _provider.Mappings.TryAdd(".rar", "application/vnd.rar");
            _provider.Mappings.TryAdd(".7z", "application/x-7z-compressed");
            _provider.Mappings.TryAdd(".mp4", "video/mp4");
            _provider.Mappings.TryAdd(".mov", "video/quicktime");
            _provider.Mappings.TryAdd(".avi", "video/x-msvideo");
            _provider.Mappings.TryAdd(".mkv", "video/x-matroska");
            _provider.Mappings.TryAdd(".png", "image/png");
            _provider.Mappings.TryAdd(".jpg", "image/jpeg");
            _provider.Mappings.TryAdd(".jpeg", "image/jpeg");
            _provider.Mappings.TryAdd(".gif", "image/gif"); 
            _provider.Mappings.TryAdd(".webp", "image/webp");
            _provider.Mappings.TryAdd(".svg", "image/svg+xml");
            _provider.Mappings.TryAdd(".txt", "text/plain");
            _provider.Mappings.TryAdd(".rtf", "application/rtf");
            _provider.Mappings.TryAdd(".xml", "application/xml");
        }

        /// <summary>
        /// Returns the MIME type based on the file name's extension.
        /// </summary>
        /// <param name="fileName">The file name including its extension (e.g. 'document.pdf').</param>
        /// <returns>The corresponding MIME type, or 'application/octet-stream' if unknown.</returns>
        public static string GetMimeType(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "application/octet-stream";

            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(extension))
                return "application/octet-stream";

            // Check the cache to improve performance
            if (_mimeCache.TryGetValue(extension, out var cachedType))
                return cachedType;

            // Use the provider to get the content type
            if (!_provider.TryGetContentType(fileName, out var contentType))
                contentType = "application/octet-stream";

            _mimeCache[extension] = contentType; // Cache it for future use
            return contentType;
        }
    }
}
