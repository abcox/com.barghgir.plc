using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.data.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Source { get; set; }
        public int? DurationSeconds { get; set; }
        public ContentType TypeId { get; set; } = ContentType.Audio;
        [IgnoreDataMember]
        public int Index { get; set; }
    }
}
