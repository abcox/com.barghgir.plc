using com.barghgir.plc.api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Options;
using com.barghgir.plc.infra.common.Encryption;
using com.barghgir.plc.infra.Security.Token;
using com.barghgir.plc.common.Configuration;
using com.barghgir.plc.data.Models;
using Microsoft.EntityFrameworkCore;
using Context = com.barghgir.plc.data.Context;
using com.barghgir.plc.data.Helpers;

namespace com.barghgir.plc.api.Controllers;

[Route("[controller]")]
[ApiController]
public class CommunityController : ControllerBase
{
    private readonly Context.CcaDevContext dbContext;
    private readonly ILogger<CommunityController> logger;
    private readonly SecurityOptions options;
    private readonly IJwtTokenGenerator tokenGenerator;

    public CommunityController(
        Context.CcaDevContext dbContext,
        ILogger<CommunityController> logger,
        IOptions<ApiOptions> options,
        IJwtTokenGenerator tokenGenerator
        )
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.options = options.Value.Security;
        this.tokenGenerator = tokenGenerator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [Route("auth/signin", Name = "SignIn")]
    public IActionResult SignIn(string email, string password, bool isPasswordClear = false)
    {
        string token;
        try
        {
            Context.Member? member = dbContext.Members.FirstOrDefault(x => x.Email.Equals(email));
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
                    dbContext.SaveChanges();
                }
                logger.LogError("Invalid username or password");
                return BadRequest("Invalid username or password");
            }
            member.LastSignInDate = DateTime.UtcNow;
            member.FailedSignInCount = 0;
            member.LockDate = null;

            token = tokenGenerator.CreateToken(
                email, id: (member?.Id ?? 0).ToString(),
                isAdmin: member?.IsAdmin ?? false
                ).Result;
            //member.LastToken = token;
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest("Oops! Something bad happened. Try againg?");
        }
        return Ok(token);
    }

    [HttpGet]
    [Route("admin/member/list", Name = "GetMemberList")]
    public IEnumerable<Context.Member>? GetMemberList()
    {
        var members = dbContext.Members.ToList();
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
        Context.Member? member = dbContext.Members.FirstOrDefault(x => x.Id.Equals(memberId));
        if (member == null)
        {
            logger.LogError("Member not found");
            return BadRequest("Member not found");
        }
        member.FailedSignInCount = 0;
        member.LastFailedSignInDate = null;
        member.LastPasswordUpdate = null;   // todo: at client, when null or > policy days, force password update
        member.LockDate = null;
        dbContext.Members.Update(member);
        dbContext.SaveChanges();
        return Ok(member);
    }

    [HttpPost]
    [Route("admin/member/create", Name = "AdminMemberCreate")]
    public IActionResult AdminMemberCreate(Context.Member member, string? password)
    {
        if (!DataValidationHelper.IsValidEmail(member?.Email))
        {
            logger.LogError("Email not valid");
            return BadRequest();
        }
        if (dbContext.Members.Any(x => x.Email.Equals(member.Email)))
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
        dbContext.Members.Add(member);
        dbContext.SaveChanges();
        return Ok(member);
    }

    [HttpPost]
    [Route("member/create", Name = "CreateMember")]
    public IActionResult CreateMember(Context.Member member)
    {
        if (member == null)
        {
            logger.LogError("Member required");
            return BadRequest();
        }
        // todo: best do this check at the client
        if (!DataValidationHelper.IsValidEmail(member?.Email))
        {
            logger.LogError("Email not valid");
            return BadRequest();
        }
        var memberExists = dbContext.Members.Any(x => x.Email.Equals(member.Email));
        if (memberExists)
        {
            logger.LogError("Member exists");
            return BadRequest("Member exists");
        }
        member.LastSignInDate = null;
        member.LockDate = null;
        member.VerifyDate = null;
        dbContext.Members.Add(member);
        dbContext.SaveChanges();
        return Ok(member);
    }

    [HttpPut]
    [Route("member/profile/update", Name = "UpdateMemberProfile")]
    public IActionResult UpdateMemberProfile(Context.Member? member)
    {
        if (member == null)
        {
            logger.LogError("Member required");
            return BadRequest("Member required");
        }
        Context.Member? updateMember = member;
        member = dbContext.Members.FirstOrDefault(x => x.Id.Equals(member.Id) && x.LockDate < DateTime.UtcNow);
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
        dbContext.Members.Update(member);
        dbContext.SaveChanges();
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
        Context.Member? member = dbContext.Members.FirstOrDefault(x => x.Email.Equals(email));
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
            dbContext.Members.Update(member);
            dbContext.SaveChanges();
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
    public IActionResult VerifyMember(Context.Member? member)
    {
        if (member == null)
        {
            logger.LogError("Member required");
            return BadRequest("Member required");
        }
        member = dbContext.Members.FirstOrDefault(x => x.Id.Equals(member.Id));
        if (member == null)
        {
            logger.LogError("Member not found");
            return BadRequest("Member not found");
        }
        member.VerifyDate = DateTime.UtcNow;
        dbContext.Members.Update(member);
        dbContext.SaveChanges();
        return Ok(member);
    }
}
