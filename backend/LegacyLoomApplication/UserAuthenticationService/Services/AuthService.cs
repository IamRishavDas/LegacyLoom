using AuthenticationManager;
using Microsoft.EntityFrameworkCore;
using ServiceResponseShared;
using System;
using System.Net;
using System.Threading.Tasks;
using UserAuthenticationService.DTOs.UserAuthenticationDTOs;
using UserAuthenticationService.Models;
using UserAuthenticationService.Repositories;

namespace UserAuthenticationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationTokenProvider _tokenProvider;

        public AuthService(IUserRepository userRepository, AuthenticationTokenProvider authenticationTokenProvider)
        {
            _userRepository = userRepository;
            _tokenProvider = authenticationTokenProvider;
        }

        private async Task<(bool, User?)> ValidateUserCredentialsUsingUserNameAndPassword(UserLoginRequest userLoginRequest)
        {
            try
            {
                var user = await _userRepository.GetUserByUserName(userLoginRequest.UserName);
                if (user == null || user.Password != userLoginRequest.Password)
                {
                    return (false, null);
                }

                return (true, user);
            }
            catch
            {
                return (false, null);
            }
        }

        private async Task<(bool, User?)> ValidateUserCredentialsUsingEmailAndPassword(UserLoginRequest userLoginRequest)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(userLoginRequest.Email.ToLower());
                if (user == null || user.Password != userLoginRequest.Password)
                {
                    return (false, null);
                }

                return (true, user);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task<ServiceResponse<UserLoginResponse>> GenerateTokenForLogin(UserLoginRequest userLoginRequest, bool usingUserNameAndPassword = true)
        {
            try
            {
                if (userLoginRequest == null)
                {
                    return ServiceResponse<UserLoginResponse>.Failure(
                        "Login request cannot be null",
                        (int)HttpStatusCode.BadRequest);
                }

                var (isValidUser, user) = usingUserNameAndPassword
                    ? await ValidateUserCredentialsUsingUserNameAndPassword(userLoginRequest)
                    : await ValidateUserCredentialsUsingEmailAndPassword(userLoginRequest);

                if (!isValidUser || user == null)
                {
                    return ServiceResponse<UserLoginResponse>.Failure(
                        "Invalid username/email or password",
                        (int)HttpStatusCode.BadRequest);
                }

                var (token, tokenExpiryTime) = _tokenProvider.GenerateJwtToken(user.Username, user.Role.ToString(), isTokenGeneratedWhileLogin: true);
                var response = new UserLoginResponse
                {
                    Id = user.Id,
                    UserName = user.Username,
                    Token = token,
                    ExpiresIn = (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds
                };

                return ServiceResponse<UserLoginResponse>.SuccessResult(
                    response,
                    (int)HttpStatusCode.OK,
                    "Login successful");
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserLoginResponse>.Failure(
                    "Error during login",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public ServiceResponse<UserLoginResponse> GenerateTokenForLogout()
        {
            try
            {
                var (token, tokenExpiryTime) = _tokenProvider.GenerateJwtToken("", "", isTokenGeneratedWhileLogin: false);
                var response = new UserLoginResponse
                {
                    Id = Guid.Empty,
                    UserName = "",
                    Token = token,
                    ExpiresIn = (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds
                };

                return ServiceResponse<UserLoginResponse>.SuccessResult(
                    response,
                    (int)HttpStatusCode.OK,
                    "Logout token generated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserLoginResponse>.Failure(
                    "Error during logout",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }
    }
}