using FirebaseAdmin.Auth;

namespace PropertyAPI.Services;

public class TokenService
{
    public async Task<string> GetUid(string Token)
    {
        var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(Token);
        return decoded.Uid;
    }
}