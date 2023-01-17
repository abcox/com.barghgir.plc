using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.data.Models;

public class Content
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Source { get; set; }
    public int? DurationSeconds { get; set; }
    public string? DurationDisplay { get; set; }
    public ContentType TypeId { get; set; } = ContentType.audio;
    [IgnoreDataMember]
    public int Index { get; set; }

    public static Content GetContent(Context.Content content)
    {
        if (content == null) return new Content();
        else return new Content
        {
            Id = content.Id,
            Title = content.Title,
            Source = content.Source,
            DurationSeconds = content.DurationSeconds
        };
    }
}