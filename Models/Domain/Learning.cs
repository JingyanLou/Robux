namespace Backend.Models.Domain;

// Learning.cs
public class Learning
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign key for user
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    // Navigation properties
    public ICollection<LearningImage> Images { get; set; }
    public ICollection<LearningTag> LearningTags { get; set; }
}
