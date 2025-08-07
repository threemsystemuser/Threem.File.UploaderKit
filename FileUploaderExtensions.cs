using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Threem.File.UploaderKit.Interfaces;
using Threem.File.UploaderKit.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Threem.File.UploaderKit
{
    public static class FileUploaderExtensions
    {
        public static IServiceCollection AddFileUploader(this IServiceCollection services, FileUploaderSettings settings)
        {
            services.AddSingleton<IFileUploader>(new FileUploaderService(settings));
            return services;
        }
    }
}
