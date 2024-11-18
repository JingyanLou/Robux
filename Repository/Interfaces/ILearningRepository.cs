using Backend.Models.Domain;

namespace Backend.Repository.Interfaces;

public interface ILearningRepository : IBaseRepository<Learning>
{
    Task<IEnumerable<Learning>> GetUserLearningsAsync(string userId);
    Task<Learning> GetLearningWithDetailsAsync(int id);
}