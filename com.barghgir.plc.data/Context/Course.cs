using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace com.barghgir.plc.data.Context;

public partial class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Category { get; set; }

    public int? ImageId { get; set; }

    public int? ContentTypeId { get; set; }

    public virtual ICollection<CourseContent> CourseContents { get; } = new List<CourseContent>();

    [NotMapped]
    public List<Models.Content> Content { get; set; } // temporary

    public static implicit operator Course(Models.Course v)
    {
        throw new NotImplementedException();
    }
}
