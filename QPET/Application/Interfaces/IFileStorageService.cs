using QPET.Application.DTOs;

namespace QPET.Application.Interfaces
{
    public interface IFileStorageService
    {
        bool Validate(UploadedFile file);

        Task<string> SaveAsync(
            UploadedFile file,
            string folder);

        Task<Stream?> OpenReadAsync(string filePath);

        Task DeleteAsync(string filePath);
    }
}