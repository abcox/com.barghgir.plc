using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.data.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public DateTime? LockOutDate { get; set; }
        public DateTime? LastVerifyDate { get; set; }
    }
}
