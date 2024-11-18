using Microsoft.AspNetCore.Identity;

namespace Backend.Models.Domain;

public class ApplicationUser : IdentityUser
{
    public ICollection<Learning> Learnings { get; set; }
}