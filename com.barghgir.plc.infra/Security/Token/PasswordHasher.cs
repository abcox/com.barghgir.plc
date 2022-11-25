using System;
using System.Security.Cryptography;
using System.Text;

namespace com.barghgir.plc.infra.Security.Token;

public class PasswordHasher : IPasswordHasher
{
    public string CreateSalt(int size)
    {
        var rng = new RNGCryptoServiceProvider();
        var buff = new byte[size];
        rng.GetBytes(buff);
        return Convert.ToBase64String(buff);
    }

    public string CreatePasswordHash(string password, string salt)
    {
        var saltAndPwd = string.Concat(password, salt);
        SHA1 sha = new SHA1Managed();
        var ae = new ASCIIEncoding();

        var data = ae.GetBytes(saltAndPwd);

        var digest = sha.ComputeHash(data);

        return GetAsHexaDecimal(digest);
    }

    private static string GetAsHexaDecimal(byte[] bytes)
    {
        var s = new StringBuilder();
        var length = bytes.Length;
        for (var n = 0; n < length; n++)
        {
            s.Append($"{bytes[n],2:x}".Replace(" ", "0"));
        }

        return s.ToString();
    }
}
