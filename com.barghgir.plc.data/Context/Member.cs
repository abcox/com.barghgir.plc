using System;
using System.Collections.Generic;

namespace com.barghgir.plc.data.Context;

public partial class Member
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? VerifyDate { get; set; }

    public DateTime? LockDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastSignInDate { get; set; }

    public DateTime? LastPasswordUpdate { get; set; }

    public string? VerifyCode { get; set; }

    public bool? IsAdmin { get; set; }

    public int FailedSignInCount { get; set; }

    public DateTime? LastFailedSignInDate { get; set; }
}
