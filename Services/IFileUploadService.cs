using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task DeleteFileAsync(string fileName);
        Task<List<string>> ListFilesAsync();
    }
}