using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.data.Models.Response
{
    public class PagedResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
    }
}
