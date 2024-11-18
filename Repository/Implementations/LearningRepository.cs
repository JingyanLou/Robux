using Backend.Data;
using Backend.Models.Domain;
using Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implementations;

public class LearningRepository : BaseRepository<Learning>, ILearningRepository
{
    public LearningRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Learning>> GetUserLearningsAsync(string userId)
    {
        return await _context.Learnings
            .Include(l => l.Images)
            .Include(l => l.LearningTags)
            .ThenInclude(lt => lt.Tag)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<Learning> GetLearningWithDetailsAsync(int id)
    {
        return await _context.Learnings
            .Include(l => l.Images)
            .Include(l => l.LearningTags)
            .ThenInclude(lt => lt.Tag)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}