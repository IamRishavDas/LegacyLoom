
namespace AuthenticationManager.TokenProviderInterface
{
    public interface ITokenProvider
    {
        (string, DateTime) GenerateJwtToken(Guid userId, string userName, string userRole, bool isTokenGeneratedWhileLogin = true);
    }
}
