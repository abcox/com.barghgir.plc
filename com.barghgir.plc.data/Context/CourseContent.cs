using System;
using System.Collections.Generic;

namespace com.barghgir.plc.data.Context;

public partial class CourseContent
{
    public int Id { get; set; }

    public int? CourseId { get; set; }

    public int? ContentId { get; set; }

    public int? SortOrder { get; set; }

    public virtual Content? Content { get; set; }

    public virtual Course? Course { get; set; }
}
