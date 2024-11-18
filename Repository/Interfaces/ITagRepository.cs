using Backend.Models.Domain;

namespace Backend.Repository.Interfaces;

public interface ITagRepository : IBaseRepository<Tag>
{
    Task<Tag> GetOrCreateTagAsync(string tagName);
    Task<IEnumerable<Tag>> GetTagsByNamesAsync(IEnumerable<string> tagNames);
}