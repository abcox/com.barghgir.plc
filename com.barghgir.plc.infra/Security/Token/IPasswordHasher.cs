namespace com.barghgir.plc.infra.Security.Token;

public interface IPasswordHasher
{
    string CreateSalt(int size);
    string CreatePasswordHash(string password, string salt);
}