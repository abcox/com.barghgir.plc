using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.data.Models
{
    public class ContentType : Enumeration
    {
        public static ContentType Audio = new(1, nameof(Audio));

        public ContentType(int id, string name) : base(id, name)
        {
        }
    }
}
