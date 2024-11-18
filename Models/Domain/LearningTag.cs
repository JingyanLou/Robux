namespace Backend.Models.Domain;

// LearningTag.cs (Junction table for many-to-many relationship)
public class LearningTag
{
    public int LearningId { get; set; }
    public Learning Learning { get; set; }
    
    public int TagId { get; set; }
    public Tag Tag { get; set; }
}