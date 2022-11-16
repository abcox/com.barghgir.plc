namespace com.barghgir.plc.api.Models
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<MediaTrack> MediaTracks { get; set; }
    }
}
