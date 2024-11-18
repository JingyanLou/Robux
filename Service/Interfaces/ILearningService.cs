using Backend.Models.DTOs.Learning;

namespace Backend.Service.Interfaces;

// Service/Interfaces/ILearningService.cs
public interface ILearningService
{
    Task<LearningResponseDto> CreateLearningAsync(CreateLearningDto dto, string userId);
    Task<LearningResponseDto> UpdateLearningAsync(int id, UpdateLearningDto dto, string userId);
    Task DeleteLearningAsync(int id, string userId);
    Task<LearningResponseDto> GetLearningAsync(int id);
    Task<IEnumerable<LearningResponseDto>> GetUserLearningsAsync(string userId);
}