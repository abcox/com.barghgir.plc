using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.data.Models.Request
{
    public class CourseFilter<T> : Filter<T>
    {
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
