

# Threem.File.UploaderKit
 
A robust file upload solution for .NET applications that handles file storage in Azure Blob Storage with metadata management in either MongoDB or SQL Server.
 
## Features
 
- **Dual Storage Support**: Store files in Azure Blob Storage while maintaining metadata in **either**:

  - MongoDB *(for document-based flexibility)*, **or**

  - SQL Server *(for relational data needs)*

- **Automatic Configuration**: Simple setup with just one line of code

- **Comprehensive File Handling**:

  - SHA256 hash generation

  - MIME type detection

  - File size tracking

  - Custom metadata support

- **Logging Integration**: Built-in NLog support with configurable log directory

- **Demo Project Available**: Check out the [Threem.File.UploaderKit Demo API](https://github.com/threemsystemuser/UploaderKitTestApi.git) for a working example
 
## Installation
 
```bash

dotnet add package Threem.File.UploaderKit

```
 
⚡ Quick Start
 
### 1. Configure appsettings.json
 
```json

{

  "FileUploader": {

    "StoreType": "SqlServer", // or "MongoDB"

    "Mongo": {

      "ConnectionString":"your-mongodb-connection-string",

      "Database": "FileMetadataDB",

      "Collection": "FileMetadata"

    },
 
    "Sql": {

      "ConnectionString": "Your_SQL_Connection_String"

    },
 
    "AzureBlobConnectionString": "Your_Azure_Blob_Connection_String",

    "AzureBlobContainer": "your-container-name",

    "LogDirectory": "C:\\Logs\\FileUploader"

  }

}

```
 
✅ Tip: Choose the StoreType based on where you want to store your file metadata (SQL Server or MongoDB).
 
### 2. Set up your Program.cs
 
```csharp
 
var settings = builder.Configuration.GetSection("FileUploader").Get<FileUploaderSettings>();
 
// Add services to the container

builder.Services.AddControllers();
 
// Configure File Uploader with automatic setup

builder.Services.AddFileUploader(settings);

```
 
💡 The AddFileUploader() method automatically reads your config and sets everything up.
 
### 3. Create your controller
 
Here’s how to implement an endpoint that accepts files and uploads them:
 
```csharp

[ApiController]

[Route("[controller]")]

[Consumes("multipart/form-data")]

public class FileUploadController : ControllerBase

{

    private readonly IFileUploader _fileUploader;
 
    public FileUploadController(IFileUploader fileUploader)

    {

        _fileUploader = fileUploader;

    }
 
    [HttpPost("upload")]

    public async Task<IActionResult> Upload(

        [FromForm] IFormFile file,

        [FromQuery] string clientId)

    {

        await _fileUploader.UploadAsync(file, clientId, new Dictionary<string, object>

        {

            { "status", "uploaded" },

            { "uploadedBy", "API" }

        });

        return Ok(new { Message = "Upload successful" });

    }

}

```
 
📌 You can enrich the uploaded file with metadata using the dictionary passed to UploadAsync.
 
---
 
## ⚙️ Configuration Options
 
### 📋 FileUploaderSettings
 
| Property                    | Type   | Required | Description                                     |

| --------------------------- | ------ | -------- | ----------------------------------------------- |

| `StoreType`                 | enum   | ✅ Yes    | MongoDB or SqlServer                           |

| `AzureBlobConnectionString` | string | ✅ Yes    | Azure Blob Storage connection string           |

| `AzureBlobContainer`        | string | ✅ Yes    | Name of the target container                   |

| `LogDirectory`              | string | ✅ Yes    | Directory path for logs                        |
 
### 📋 MongoDB Config (when StoreType = MongoDB)
 
| Property           | Type   | Required | Description        |

| ------------------ | ------ | -------- | ------------------ |

| `ConnectionString` | string | ✅ Yes    | MongoDB connection |

| `Database`         | string | ✅ Yes    | Database name      |

| `Collection`       | string | ✅ Yes    | Collection name    |
 
### 📋 SQL Server Config (when StoreType = SqlServer)
 
| Property           | Type   | Required | Description           |

| ------------------ | ------ | -------- | --------------------- |

| `ConnectionString` | string | ✅ Yes    | SQL Server connection |
 
---
 
## 🧱 Database Schema Requirements
 
### 📋 MongoDB
 
Automatically creates the required collection. No schema setup needed.
 
### 🧱 SQL Server
 
Create this table in your database:
 
```sql

CREATE TABLE FileMetadata (

    Id NVARCHAR(255) PRIMARY KEY,

    FileName NVARCHAR(255) NOT NULL,

    ClientId NVARCHAR(255) NOT NULL,

    BlobUri NVARCHAR(MAX) NOT NULL,

    UploadDate DATETIME2 NOT NULL,

    FileSize BIGINT NOT NULL,

    MimeType NVARCHAR(100) NOT NULL,

    SHA256 NVARCHAR(64) NOT NULL,

    AdditionalData NVARCHAR(MAX) NULL

);

```
 
---
 
## 📚 Logging Configuration (NLog)
 
Add `nlog.config` to your project.
 
Register NLog in Program.cs:
 
```csharp

builder.Logging.AddNLog("nlog.config");

```
 
---
 
## 🤝 Contributing
 
Contributions are welcome! Please:
 
- Code follows existing style

- New features include tests

- Documentation is updated
 
---
 
## 🐞 Issues & Support
 
If you face any issues or need support, please reach out via email:  

📧 info@threemsolutions.com
 
---
 
## 📜 License
 
This package is licensed under the MIT License.

 