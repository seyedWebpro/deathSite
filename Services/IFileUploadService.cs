using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace api.Services
{
   public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string entityType, int entityId);
        Task<List<string>> UploadMultipleFilesAsync(List<IFormFile> files, string entityType, int entityId);
        Task DeleteFileAsync(string fileName, string entityType, int entityId);
        Task<List<string>> ListFilesAsync(string entityType, int entityId);
    }
}
