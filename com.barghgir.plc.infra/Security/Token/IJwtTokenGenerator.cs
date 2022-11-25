using System.Threading.Tasks;

namespace com.barghgir.plc.infra.Security.Token;

public interface IJwtTokenGenerator
{
    Task<string> CreateToken(string email, string id, bool isAdmin);
}