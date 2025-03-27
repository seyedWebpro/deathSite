using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace api.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly string _uploadsFolder;
        private readonly long _maxFileSize = 500 * 1024 * 1024; // 500 MB
        private readonly List<string> _allowedFileTypes = new List<string>
        {
            "image/jpeg", "image/png", "image/gif",
            "video/mp4",
            "audio/mpeg", "audio/wav"
        };

        public FileUploadService()
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string entityType, int entityId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("فایل انتخاب نشده است.");
            }

            if (!_allowedFileTypes.Contains(file.ContentType))
            {
                throw new ArgumentException("نوع فایل مجاز نیست.");
            }

            if (file.Length > _maxFileSize)
            {
                throw new ArgumentException("حجم فایل از حد مجاز بیشتر است.");
            }

            string category = GetCategory(file.ContentType);
            string entityFolder = Path.Combine(_uploadsFolder, entityType, entityId.ToString(), category);

            if (!Directory.Exists(entityFolder))
            {
                Directory.CreateDirectory(entityFolder);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(entityFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{entityType}/{entityId}/{category}/{fileName}";
        }

        public async Task<List<string>> UploadMultipleFilesAsync(List<IFormFile> files, string entityType, int entityId)
        {
            if (files == null || !files.Any())
            {
                throw new ArgumentException("هیچ فایلی ارسال نشده است.");
            }

            List<string> fileUrls = new();

            foreach (var file in files)
            {
                fileUrls.Add(await UploadFileAsync(file, entityType, entityId));
            }

            return fileUrls;
        }

        public async Task DeleteFileAsync(string fileName, string entityType, int entityId)
        {
            try
            {
                string categoryFolder = GetCategoryFolder(entityType, entityId, fileName);

                if (!Directory.Exists(categoryFolder))
                {
                    throw new DirectoryNotFoundException("پوشه مربوطه پیدا نشد.");
                }

                string filePath = Path.Combine(categoryFolder, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    throw new FileNotFoundException("فایل پیدا نشد.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"در هنگام حذف فایل خطایی رخ داده است: {ex.Message}", ex);
            }
        }

        public Task<List<string>> ListFilesAsync(string entityType, int entityId)
        {
            string entityFolder = Path.Combine(_uploadsFolder, entityType, entityId.ToString());

            if (!Directory.Exists(entityFolder))
            {
                return Task.FromResult(new List<string>());
            }

            var files = Directory.GetFiles(entityFolder, "*", SearchOption.AllDirectories)
                .Select(file => file.Replace(_uploadsFolder, "").Replace("\\", "/"))
                .ToList();

            return Task.FromResult(files);
        }

        private string GetCategory(string contentType)
        {
            if (contentType.StartsWith("image/")) return "images";
            if (contentType.StartsWith("video/")) return "videos";
            if (contentType.StartsWith("audio/")) return "audios";
            throw new ArgumentException("نوع فایل مجاز نیست.");
        }

        private string GetCategoryFolder(string entityType, int entityId, string fileName)
        {
            string fileType = Path.GetExtension(fileName).ToLower();
            string category = fileType switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" => "images",
                ".mp4" => "videos",
                ".mp3" or ".wav" => "audios",
                _ => throw new ArgumentException("نوع فایل مجاز نیست.")
            };

            return Path.Combine(_uploadsFolder, entityType, entityId.ToString(), category);
        }

        public async Task<string> SaveQRCodeAsync(byte[] fileBytes, string entityType, int entityId, string fileName)
        {
            // تعریف دسته‌بندی مخصوص QR Code (برای مثال: "qrcodes")
            string category = "qrcodes";
            // مسیر پوشه: wwwroot/uploads/{entityType}/{entityId}/qrcodes
            string entityFolder = Path.Combine(_uploadsFolder, entityType, entityId.ToString(), category);

            if (!Directory.Exists(entityFolder))
            {
                Directory.CreateDirectory(entityFolder);
            }

            string filePath = Path.Combine(entityFolder, fileName);
            await File.WriteAllBytesAsync(filePath, fileBytes);

            // بازگرداندن URL نسبی فایل ذخیره شده
            return $"/uploads/{entityType}/{entityId}/{category}/{fileName}";
        }

        public async Task DeleteQRCodeAsync(string fileName, string entityType, int entityId)
        {
            try
            {
                // دسته‌بندی QR Code
                string category = "qrcodes";
                // مسیر پوشه: wwwroot/uploads/{entityType}/{entityId}/qrcodes
                string entityFolder = Path.Combine(_uploadsFolder, entityType, entityId.ToString(), category);

                string filePath = Path.Combine(entityFolder, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    throw new FileNotFoundException("فایل QR Code پیدا نشد.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"در هنگام حذف فایل QR Code خطایی رخ داده است: {ex.Message}", ex);
            }
        }

    }
}
