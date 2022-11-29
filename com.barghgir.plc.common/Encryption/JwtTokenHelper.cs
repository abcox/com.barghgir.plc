using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace com.barghgir.plc.common.Encryption
{
    public static class JwtTokenHelper
    {
        public static JwtSecurityToken? GetJwtSecurityToken(string jwt)
        {
            if (string.IsNullOrEmpty(jwt)) return null;
            return new JwtSecurityTokenHandler().ReadToken(jwt) as JwtSecurityToken;
        }

        public static bool IsTokenValid(string jwt)
        {
            JwtSecurityToken? token = GetJwtSecurityToken(jwt);
            return IsTokenValid(token);
        }

        public static bool IsTokenValid(JwtSecurityToken? token)
        {
            return token != null &&
                token.ValidFrom < DateTime.UtcNow &&
                token.ValidTo > DateTime.UtcNow;
        }

        public static bool IsAdmin(string? jwt)
        {
            var roles = GetRoles(jwt);
            return roles?.Contains("admin") ?? false;
        }

        public static List<string>? GetRoles(string? jwt)
        {
            if (jwt == null) return null;
            var claims = GetJwtTokenClaims(jwt);
            var roles = GetRoles(claims);
            return roles;
        }

        public static List<string>? GetRoles(Dictionary<string, string>? claims)
        {
            return claims?.Where(x => x.Key == "role")?.Select(x => x.Value)?.ToList();
        }

        public static Dictionary<string, string>? GetJwtTokenClaims(string jwt)
        {
            Dictionary<string, string>? roles = null;
            try
            {
                JwtSecurityToken? token = GetJwtSecurityToken(jwt);

                if (token != null && !IsTokenValid(token))
                    return null;

                roles = token?.Claims
                    .ToDictionary(x => x.Type, x => x.Value,
                        StringComparer.OrdinalIgnoreCase);

                return roles;
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}