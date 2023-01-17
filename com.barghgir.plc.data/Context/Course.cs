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

    [NotMapped]
    public string? ImageUrl { get; set; }

    public int? ContentTypeId { get; set; }

    public virtual ICollection<CourseContent> CourseContents { get; } = new List<CourseContent>();

    public virtual IQueryable<Content> Content => CourseContents.Where(x => x.CourseId.Equals(this.Id)).Select(x => x.Content).AsQueryable();
}
