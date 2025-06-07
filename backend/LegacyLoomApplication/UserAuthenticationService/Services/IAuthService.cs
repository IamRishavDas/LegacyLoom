using ServiceResponseShared;
using UserAuthenticationService.DTOs.UserAuthenticationDTOs;

namespace UserAuthenticationService.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<UserLoginResponse>> GenerateTokenForLogin(UserLoginRequest userLoginRequest, bool usingUserNameAndPassword = true);
        ServiceResponse<UserLoginResponse> GenerateTokenForLogout();
    }
}