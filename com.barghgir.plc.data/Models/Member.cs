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
        public string? Name { get; set; }
        public string Email { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public DateTime? LockDate { get; set; }
        public string? Password { get; set; }
        public DateTime? LastPasswordUpdate { get; set; }
        public DateTime? LastFailedSignInDate { get; set; }
        public int? FailedSignInCount { get; set; }
        public string? VerifyCode { get; set; }
        public DateTime? VerifyDate { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
