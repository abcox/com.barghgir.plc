namespace com.barghgir.plc.data.Models;

public class Course
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Category { get; set; }
    public string? ImageSource { get; set; } // URL
    public List<MediaTrack>? MediaTracks { get; set; }
}