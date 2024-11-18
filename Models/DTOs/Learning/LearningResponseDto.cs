namespace Backend.Models.DTOs.Learning;

// Learning/LearningResponseDto.cs
public class LearningResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Username { get; set; }
    public List<string> ImageUrls { get; set; }
    public List<string> Tags { get; set; }
}