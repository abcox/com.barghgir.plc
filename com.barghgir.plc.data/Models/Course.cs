namespace com.barghgir.plc.data.Models;

public class Course
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Category { get; set; }
    public int? ImageId { get; set; }
    public string? ImageUrl { get; set; }
    public List<Content>? Content { get; set; }
}