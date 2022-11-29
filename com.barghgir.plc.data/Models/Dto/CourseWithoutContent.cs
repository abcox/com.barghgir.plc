using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace com.barghgir.plc.data.Models;

public partial class CourseWithoutContent
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Category { get; set; }
    public int? ImageId { get; set; }
    public string? ImageUrl { get; set; }
    //public int ContentTypeId { get; set; }
    //public ContentType? ContentType { get; set; }
    public string? ContentTypeIcon { get; set; }
}
