using ServiceResponseShared;
using UserAuthenticationService.DTOs.UserAuthenticationDTOs;

namespace UserAuthenticationService.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<UserLoginResponse>> GenerateTokenForLoginByEmail(UserLoginRequestByEmail userLoginRequest);
        Task<ServiceResponse<UserLoginResponse>> GenerateTokenForLoginByUsername(UserLoginRequestByUsername userLoginRequest);
        ServiceResponse<UserLoginResponse> GenerateTokenForLogout();
    }
}