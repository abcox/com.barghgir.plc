using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace com.barghgir.plc.data.Context;

public partial class Content
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Source { get; set; }

    public int? DurationSeconds { get; set; }

    [NotMapped]
    public int? Index { get; set; }

    public virtual ICollection<CourseContent> CourseContents { get; } = new List<CourseContent>();

    //[JsonIgnore]
    public virtual ICollection<Course> Courses { get; } = new List<Course>();
}
