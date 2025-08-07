using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Threem.File.UploaderKit.Models;

namespace Threem.File.UploaderKit.Interfaces
{
    public interface IMetadataStore
    {
        Task InsertAsync(FileMetadata metadata);
    }
}
