using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManager.TokenProviderInterface
{
    public interface ITokenProvider
    {
        (string, DateTime) GenerateJwtToken(string userName, string userRole, bool isTokenGeneratedWhileLogin = true);
    }
}
