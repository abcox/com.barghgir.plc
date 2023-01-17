using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace com.barghgir.plc.data.Models;

public class Course
{
    public Course() { }

    public Course(Context.Course? course)
    {
        if (course == null) throw new ArgumentNullException();

        this.Id = course.Id;
        this.ImageId = course.ImageId;
        this.Subtitle = course.Subtitle;
        this.Title = course.Title;
        this.Category = course.Category;
        //this.Content = course.Content;
        this.ContentType = course.ContentTypeId == 1 ?
            new ContentType(1, "Audio") : new ContentType(2, "Video");
    }

    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Category { get; set; }
    public int? ImageId { get; set; }
    public string? ImageUrl { get; set; }
    public int ContentTypeId { get; set; }
    public ContentType? ContentType { get; set; }
    public ICollection<Content>? Content { get; set; }
}