using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threem.File.UploaderKit.Interfaces
{
    public interface IAzureBlobService
    {
       
            Task<string> UploadFileAsync(Stream fileStream, string fileName);
        
    }
}

