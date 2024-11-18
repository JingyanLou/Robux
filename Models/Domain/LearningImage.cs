namespace Backend.Models.Domain;

// LearningImage.cs
public class LearningImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    
    // Foreign key for learning
    public int LearningId { get; set; }
    public Learning Learning { get; set; }
}
