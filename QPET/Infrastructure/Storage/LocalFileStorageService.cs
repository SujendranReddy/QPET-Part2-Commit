using QPET.Application.DTOs;
using QPET.Application.Interfaces;

namespace QPET.Infrastructure.Storage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private const long MaximumFileSize = 10 * 1024 * 1024;

        private static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp",
                ".pdf"
            };

        private static readonly HashSet<string> AllowedContentTypes =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "image/jpeg",
                "image/png",
                "image/webp",
                "application/pdf"
            };

        private readonly string _webRootPath;

        public LocalFileStorageService(
            IWebHostEnvironment environment)
        {
            _webRootPath = environment.WebRootPath;
        }

        public bool Validate(UploadedFile file)
        {
            if (file.Length <= 0 ||
                file.Length > MaximumFileSize)
            {
                return false;
            }

            var extension =
                Path.GetExtension(file.OriginalFileName);

            return AllowedExtensions.Contains(extension) &&
                   AllowedContentTypes.Contains(file.ContentType);
        }

        public async Task<string> SaveAsync(
            UploadedFile file,
            string folder)
        {
            if (!Validate(file))
            {
                throw new InvalidOperationException(
                    "The selected file is invalid.");
            }

            var safeFolder =
                folder.Trim('/', '\\');

            var directory =
                Path.Combine(
                    _webRootPath,
                    "uploads",
                    safeFolder);

            Directory.CreateDirectory(directory);

            var extension =
                Path.GetExtension(file.OriginalFileName)
                    .ToLowerInvariant();

            var storedFileName =
                $"{Guid.NewGuid():N}{extension}";

            var physicalPath =
                Path.Combine(directory, storedFileName);

            await using var output =
                new FileStream(
                    physicalPath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None);

            await file.Content.CopyToAsync(output);

            return $"/uploads/{safeFolder}/{storedFileName}";
        }
        public Task<Stream?> OpenReadAsync(string filePath)
        {
            var physicalPath =
                ResolvePhysicalPath(filePath);

            if (physicalPath == null ||
                !File.Exists(physicalPath))
            {
                return Task.FromResult<Stream?>(null);
            }

            Stream stream =
                new FileStream(
                    physicalPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);

            return Task.FromResult<Stream?>(stream);
        }

        public Task DeleteAsync(string filePath)
        {
            var physicalPath =
                ResolvePhysicalPath(filePath);

            if (physicalPath != null &&
                File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            return Task.CompletedTask;
        }

        private string? ResolvePhysicalPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            var relativePath =
                filePath
                    .TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar);

            var physicalPath =
                Path.GetFullPath(
                    Path.Combine(
                        _webRootPath,
                        relativePath));

            var uploadsRoot =
                Path.GetFullPath(
                    Path.Combine(
                        _webRootPath,
                        "uploads"));

            if (!physicalPath.StartsWith(
                    uploadsRoot + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return physicalPath;
        }
    }
}