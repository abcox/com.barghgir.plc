using com.barghgir.plc.api.Data;
using com.barghgir.plc.data.Helpers;
using com.barghgir.plc.data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Options;
using com.barghgir.plc.infra.common.Encryption;
using com.barghgir.plc.infra.Security.Token;
using com.barghgir.plc.common.Configuration;

namespace com.barghgir.plc.api.Controllers;

[Route("[controller]")]
[ApiController]
public class CommunityController : ControllerBase
{
    private readonly AppDbContext data;
    private readonly ILogger<CommunityController> logger;
    private readonly SecurityOptions options;
    private readonly IJwtTokenGenerator tokenGenerator;

    public CommunityController(
        AppDbContext dbContext,
        ILogger<CommunityController> logger,
        IOptions<ApiOptions> options,
        IJwtTokenGenerator tokenGenerator
        )
    {
        this.logger = logger;
        this.data = dbContext;
        this.options = options.Value.Security;
        this.tokenGenerator = tokenGenerator;
    }

    [HttpPost]
    [Route("auth/signin", Name = "SignIn")]
    public IActionResult SignIn(string email, string password, bool isPasswordClear = false)
    {
        Member? member = data.Member.FirstOrDefault(x => x.Email.Equals(email));
        string protectedPassword = member?.Password != null && isPasswordClear ?
            DataProtectionHelper.EncryptDataWithAes(password.Trim(),
                options.AesEncryptionKey, options.AesEncryptionIVector) : password;
        if (string.IsNullOrEmpty(password) ||
            member == null || member.LockDate != null ||
            member?.Password != protectedPassword)
        {
            if (member != null)
            {
                member.LastFailedSignInDate = DateTime.UtcNow;
                member.FailedSignInCount++;
                if (member.FailedSignInCount >= options.FailedSignInCountMaxLimit)
                {
                    member.LockDate = DateTime.UtcNow;
                    // todo: should we notify user (and admin?)?
                }
                data.SaveChanges();
            }
            logger.LogError("Invalid username or password");
            return BadRequest("Invalid username or password");
        }
        member.LastSignInDate = DateTime.UtcNow;
        member.FailedSignInCount = 0;
        member.LockDate = null;

        string token = tokenGenerator.CreateToken(
            email, id: (member?.Id ?? 0).ToString(),
            isAdmin: member?.IsAdmin ?? false
            ).Result;
        //member.LastToken = token;

        data.SaveChanges();
        return Ok(token);
    }

    [HttpGet]
    [Route("admin/member/list", Name = "GetMemberList")]
    public IEnumerable<Member>? GetMemberList()
    {
        var members = data.Member.ToList();
        return members;
    }

    [HttpGet]
    [Route("auth/token/test", Name = "AuthTokenTest")]
    public IActionResult AuthTokenTest(string email, int? id)
    {
        var options = new JwtIssuerOptions
        {
            Audience = "",
        };
        string token = tokenGenerator.CreateToken(email,
            id: (id ?? 0).ToString(), isAdmin: false).Result;
        return Ok(token);
    }

    [HttpPut]
    [Route("admin/member/unlock", Name = "AdminMemberUnlock")]
    public IActionResult AdminMemberUnlock(int memberId)
    {
        logger.LogInformation("{method}; memberId: {memberId}", nameof(AdminMemberUnlock), memberId);
        Member? member = data.Member.FirstOrDefault(x => x.Id.Equals(memberId));
        if (member == null)
        {
            logger.LogError("Member not found");
            return BadRequest("Member not found");
        }
        member.FailedSignInCount = 0;
        member.LastFailedSignInDate = null;
        member.LastPasswordUpdate = null;   // todo: at client, when null or > policy days, force password update
        member.LockDate = null;
        data.Member.Update(member);
        data.SaveChanges();
        return Ok(member);
    }

    [HttpPost]
    [Route("admin/member/create", Name = "AdminMemberCreate")]
    public IActionResult AdminMemberCreate(Member member, string? password)
    {
        if (!DataValidationHelper.IsValidEmail(member?.Email))
        {
            logger.LogError("Email not valid");
            return BadRequest();
        }
        if (data.Member.Any(x => x.Email.Equals(member.Email)))
        {
            logger.LogError("Member exists");
            return BadRequest("Member exists");
        }
        if (!string.IsNullOrEmpty(password))
        {
            string key = options.AesEncryptionKey;
            string vector = options.AesEncryptionIVector;
            string protectedPassword =
                DataProtectionHelper.EncryptDataWithAes(
                    password.Trim(),
                    options.AesEncryptionKey,
                    options.AesEncryptionIVector);
            member.Password = protectedPassword;
        }
        data.Member.Add(member);
        data.SaveChanges();
        return Ok(member);
    }

    [HttpPost]
    [Route("member/create", Name = "CreateMember")]
    public IActionResult CreateMember(Member member)
    {
        // todo: best do this check at the client
        if (!DataValidationHelper.IsValidEmail(member?.Email))
        {
            logger.LogError("Email not valid");
            return BadRequest();
        }
        if (data.Member.Any(x => x.Email.Equals(member.Email)))
        {
            logger.LogError("Member exists");
            return BadRequest("Member exists");
        }
        member.LastSignInDate = null;
        member.LockDate = null;
        member.VerifyDate = null;
        data.Member.Add(member);
        data.SaveChanges();
        return Ok(member);
    }

    [HttpPut]
    [Route("member/profile/update", Name = "UpdateMemberProfile")]
    public IActionResult UpdateMemberProfile(Member? member)
    {
        if (member == null)
        {
            logger.LogError("Member required");
            return BadRequest("Member required");
        }
        Member? updateMember = member;
        member = data.Member.FirstOrDefault(x => x.Id.Equals(member.Id) && x.LockDate < DateTime.UtcNow);
        if (member == null)
        {
            logger.LogError("Member not found");
            return BadRequest("Member not found");
        }
        if (member.LockDate != null && member.LockDate < DateTime.UtcNow)
        {
            logger.LogError("Member account locked");
            return BadRequest("Member account locked");
        }
        member.Name = updateMember.Name;
        data.Member.Update(member);
        data.SaveChanges();
        return Ok(member);
    }

    [HttpPut]
    [Route("member/password/update", Name = "UpdateMemberPassword")]
    public IActionResult UpdateMemberPassword(string email, string protectedPasswordNew, string protectedPasswordOld)
    {
        if (string.IsNullOrEmpty(protectedPasswordNew) ||
            string.IsNullOrEmpty(protectedPasswordOld))
        {
            logger.LogError("Invalid input");
            return BadRequest("Invalid input");
        }
        Member? member = data.Member.FirstOrDefault(x => x.Email.Equals(email));
        if (member == null || (member.LockDate != null && member.LockDate < DateTime.UtcNow))
        {
            logger.LogError("Member not found");
            return BadRequest("Member not found");
        }
        if (member.LockDate != null && member.LockDate < DateTime.UtcNow)
        {
            logger.LogError("Member account locked");
            return BadRequest("Member account locked");
        }
        // todo: move check to client
        if (member.Password != protectedPasswordOld ||
            protectedPasswordNew == protectedPasswordOld    // assure password really changing
            )
        {
            logger.LogError("Invalid password");
            return BadRequest("Invalid password");
        }
        try
        {
            member.FailedSignInCount = 0;
            member.LastPasswordUpdate = DateTime.UtcNow;
            member.Password = protectedPasswordNew;
            data.Member.Update(member);
            data.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
            return BadRequest("Password update failed");
        }
        return Ok(member);
    }

    [HttpPut]
    [Route("member/verify", Name = "VerifyMember")]
    public IActionResult VerifyMember(Member? member)
    {
        if (member == null)
        {
            logger.LogError("Member required");
            return BadRequest("Member required");
        }
        member = data.Member.FirstOrDefault(x => x.Id.Equals(member.Id));
        if (member == null)
        {
            logger.LogError("Member not found");
            return BadRequest("Member not found");
        }
        member.VerifyDate = DateTime.UtcNow;
        data.Member.Update(member);
        data.SaveChanges();
        return Ok(member);
    }
}
