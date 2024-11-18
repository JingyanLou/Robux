using Backend.Data;
using Backend.Models.Domain;
using Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implementations;

public class TagRepository : BaseRepository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Tag> GetOrCreateTagAsync(string tagName)
    {
        var normalizedName = tagName.ToLower().Trim();
        var tag = await _context.Tags
            .FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedName);

        if (tag == null)
        {
            tag = new Tag { Name = normalizedName };
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        return tag;
    }

    public async Task<IEnumerable<Tag>> GetTagsByNamesAsync(IEnumerable<string> tagNames)
    {
        var normalizedNames = tagNames.Select(n => n.ToLower().Trim());
        return await _context.Tags
            .Where(t => normalizedNames.Contains(t.Name.ToLower()))
            .ToListAsync();
    }
}