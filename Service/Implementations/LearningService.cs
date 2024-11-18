using Backend.Models.Domain;
using Backend.Models.DTOs.Learning;
using Backend.Repository.Interfaces;
using Backend.Service.Interfaces;

namespace Backend.Service.Implementations;

// Service/Implementations/LearningService.cs
public class LearningService : ILearningService
{
    private readonly ILearningRepository _learningRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IImageService _imageService;

    public LearningService(
        ILearningRepository learningRepository,
        ITagRepository tagRepository,
        IImageService imageService)
    {
        _learningRepository = learningRepository;
        _tagRepository = tagRepository;
        _imageService = imageService;
    }

    public async Task<LearningResponseDto> CreateLearningAsync(CreateLearningDto dto, string userId)
    {
        var learning = new Learning
        {
            Title = dto.Title,
            Body = dto.Body,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        // Handle images
        if (dto.Images != null && dto.Images.Any())
        {
            learning.Images = new List<LearningImage>();
            foreach (var image in dto.Images)
            {
                var imageUrl = await _imageService.UploadImageAsync(image);
                learning.Images.Add(new LearningImage { ImageUrl = imageUrl });
            }
        }

        // Handle tags
        if (dto.Tags != null && dto.Tags.Any())
        {
            learning.LearningTags = new List<LearningTag>();
            foreach (var tagName in dto.Tags)
            {
                var tag = await _tagRepository.GetOrCreateTagAsync(tagName);
                learning.LearningTags.Add(new LearningTag { Tag = tag });
            }
        }

        await _learningRepository.AddAsync(learning);
        var createdLearning = await _learningRepository.GetLearningWithDetailsAsync(learning.Id);
        return MapToResponseDto(createdLearning);
    }

    // Implement other methods (UpdateLearningAsync, DeleteLearningAsync, etc.)
    // ...
    
    public async Task<LearningResponseDto> UpdateLearningAsync(int id, UpdateLearningDto dto, string userId)
    {
        var learning = await _learningRepository.GetLearningWithDetailsAsync(id);
        if (learning == null)
            throw new KeyNotFoundException($"Learning with ID {id} not found");

        if (learning.UserId != userId)
            throw new UnauthorizedAccessException("You don't have permission to update this learning");

        // Update basic properties
        learning.Title = dto.Title;
        learning.Body = dto.Body;
        learning.UpdatedAt = DateTime.UtcNow;

        // Update tags
        if (dto.Tags != null)
        {
            // Remove existing tags
            learning.LearningTags.Clear();

            // Add new tags
            foreach (var tagName in dto.Tags.Distinct())
            {
                var tag = await _tagRepository.GetOrCreateTagAsync(tagName);
                learning.LearningTags.Add(new LearningTag { Tag = tag });
            }
        }

        await _learningRepository.UpdateAsync(learning);
        var updatedLearning = await _learningRepository.GetLearningWithDetailsAsync(id);
        return MapToResponseDto(updatedLearning);
    }

    public async Task DeleteLearningAsync(int id, string userId)
    {
        var learning = await _learningRepository.GetLearningWithDetailsAsync(id);
        if (learning == null)
            throw new KeyNotFoundException($"Learning with ID {id} not found");

        if (learning.UserId != userId)
            throw new UnauthorizedAccessException("You don't have permission to delete this learning");

        // Delete associated images
        if (learning.Images != null)
        {
            foreach (var image in learning.Images)
            {
                await _imageService.DeleteImageAsync(image.ImageUrl);
            }
        }

        await _learningRepository.DeleteAsync(learning);
    }

    public async Task<LearningResponseDto> GetLearningAsync(int id)
    {
        var learning = await _learningRepository.GetLearningWithDetailsAsync(id);
        if (learning == null)
            throw new KeyNotFoundException($"Learning with ID {id} not found");

        return MapToResponseDto(learning);
    }

    public async Task<IEnumerable<LearningResponseDto>> GetUserLearningsAsync(string userId)
    {
        var learnings = await _learningRepository.GetUserLearningsAsync(userId);
        return learnings.Select(MapToResponseDto);
    }
    
    private LearningResponseDto MapToResponseDto(Learning learning)
    {
        return new LearningResponseDto
        {
            Id = learning.Id,
            Title = learning.Title,
            Body = learning.Body,
            CreatedAt = learning.CreatedAt,
            UpdatedAt = learning.UpdatedAt,
            Username = learning.User?.UserName,
            ImageUrls = learning.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
            Tags = learning.LearningTags?.Select(lt => lt.Tag.Name).ToList() ?? new List<string>()
        };
    }
}