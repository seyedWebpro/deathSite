using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly string _uploadsFolder;
        private readonly long _maxFileSize = 500 * 1024 * 1024; // 500 MB
        private readonly List<string> _allowedFileTypes = new List<string> { "image/jpeg", "image/png", "image/gif", "video/mp4", "audio/mpeg", "audio/wav" };

        public FileUploadService()
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("فایل انتخاب نشده است.");
            }

            // بررسی نوع فایل
            if (!_allowedFileTypes.Contains(file.ContentType))
            {
                throw new ArgumentException("نوع فایل مجاز نیست.");
            }

            // بررسی حجم فایل
            if (file.Length > _maxFileSize)
            {
                throw new ArgumentException("حجم فایل از حد مجاز بیشتر است.");
            }

            // ایجاد نام فایل یکتا
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_uploadsFolder, fileName);

            // ذخیره فایل
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // بازگشت مسیر فایل
            return $"/uploads/{fileName}";
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadsFolder, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException("فایل پیدا نشد.");
            }
        }

        public Task<List<string>> ListFilesAsync()
        {
            var files = Directory.GetFiles(_uploadsFolder)
                .Select(Path.GetFileName)
                .ToList();
            return Task.FromResult(files);
        }
    }


}