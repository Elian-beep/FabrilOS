namespace FabrilOS.API.Services.Interfaces;

public interface IFileStorageService
{
  Task<string> UploadFileAsync(IFormFile file);
  Task<string> GetPresignedUrlAsync(string fileName);
}