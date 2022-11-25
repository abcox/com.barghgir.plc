using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using com.barghgir.plc.common.Configuration;
using Microsoft.Extensions.Options;

namespace com.barghgir.plc.infra.Security.Token;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtIssuerOptions _jwtOptions;
    //private readonly JwtSecurityTokenHandler _jwtTokenHandler;
    private readonly AppInfo info;

    public JwtTokenGenerator(
        IOptions<JwtIssuerOptions> jwtOptions,
        //,JwtSecurityTokenHandler jwtTokenHandler
        IOptions<ApiOptions> apiOptions
        )
    {
        _jwtOptions = jwtOptions.Value;
        _jwtOptions.ValidFor = new TimeSpan(0, 0, 30, 0, 0);
        //_jwtTokenHandler = jwtTokenHandler;
        this.info = apiOptions.Value.Info;
    }

    public async Task<string> CreateToken(string email, string id, bool isAdmin = false)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, "solmaz@barghgir"), // info?.ContactEmail // FIX THIS!!
            new Claim(JwtRegisteredClaimNames.Sub, id),
            new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            new Claim(JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(_jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new Claim("contact", "Solmaz Barghgir") // info.ContactName // FIX THIS!!
        };
        if (isAdmin)
            claims.Add(new Claim("role", "admin"));

        _jwtOptions.Issuer = AppDomain.CurrentDomain.FriendlyName;
        _jwtOptions.Audience = email;

        var jwt = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            _jwtOptions.NotBefore,
            _jwtOptions.Expiration,
            _jwtOptions.SigningCredentials);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
       //return _jwtTokenHandler.WriteToken(jwt);
    }
}
