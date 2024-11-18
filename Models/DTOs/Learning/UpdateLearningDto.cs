namespace Backend.Models.DTOs.Learning;

// Learning/UpdateLearningDto.cs
public class UpdateLearningDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> Tags { get; set; }
}