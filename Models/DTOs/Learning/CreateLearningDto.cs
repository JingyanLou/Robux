namespace Backend.Models.DTOs.Learning;

// Learning/CreateLearningDto.cs
public class CreateLearningDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public List<IFormFile> Images { get; set; }
    public List<string> Tags { get; set; }
}