namespace Backend.Models.Domain;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<LearningTag> LearningTags { get; set; }
}