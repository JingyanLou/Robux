using Backend.Service.Interfaces;

namespace Backend.Service.Implementations;

public class ImageService : IImageService
{
    private readonly string _uploadDirectory;
    private readonly string _baseUrl;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public ImageService(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
        _uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads", "images");
        _baseUrl = _configuration["BaseUrl"] ?? "https://yourapi.com"; // Configure this in appsettings.json

        // Ensure upload directory exists
        if (!Directory.Exists(_uploadDirectory))
        {
            Directory.CreateDirectory(_uploadDirectory);
        }
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file was provided");

        if (!IsValidImage(file))
            throw new ArgumentException("Invalid image file");

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_uploadDirectory, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the URL
        return $"{_baseUrl}/uploads/images/{fileName}";
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return;

        try
        {
            var fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
            var filePath = Path.Combine(_uploadDirectory, fileName);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
        catch (Exception)
        {
            // Log error but don't throw - we don't want to fail the whole operation if image deletion fails
        }
    }

    public bool IsValidImage(IFormFile file)
    {
        if (file == null)
            return false;

        // Check file extension
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            return false;

        // Check file signature
        try
        {
            using var reader = new BinaryReader(file.OpenReadStream());
            var signatures = new Dictionary<string, List<byte[]>>
            {
                { ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
                { ".jpg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
                { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } },
                { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } }
            };

            var headerBytes = reader.ReadBytes(8);
            return signatures[extension].Any(signature => 
                headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
        catch
        {
            return false;
        }
    }
}