namespace Backend.Service.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string imageUrl);
    bool IsValidImage(IFormFile file);
}