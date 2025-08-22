using AuthenticationManager;
using AutoMapper;
using EventModelsShared;
using Grpc.Net.Client;
using GrpcNotificationService.Protos;
using MassTransit;
using RequestFeatureShared;
using ServiceResponseShared;
using System.Net;
using UserAuthenticationService.DTOs.UserAuthenticationDTOs;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Models;
using UserAuthenticationService.Repositories;
using UserAuthenticationService.RequestFeatures;
using UserAuthenticationService.Utils;
using static GrpcNotificationService.Protos.GrpcNotificationService;

namespace UserAuthenticationService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly AuthenticationTokenProvider _tokenProvider;
        private readonly string _notificationServiceGrpcServer;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, PasswordHasher passwordHasher, IPublishEndpoint publishEndpoint, IMapper mapper, AuthenticationTokenProvider tokenProvider, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _publishEndpoint = publishEndpoint;
            _tokenProvider = tokenProvider;
            _mapper = mapper;
            _notificationServiceGrpcServer = configuration["GrpcServer:NotificationServiceGrpcServer"] ?? throw new Exception("Notificaiton Service Grpc server link not found");
        }

        public async Task<(ServiceResponse<IEnumerable<UserDTO>> users, MetaData metadata)> GetUsersAsync(UserRequestParameters userRequestParams, bool includeDeleted = false)
        {
            try
            {
                var usersWithMetaData = await _userRepository.GetUsers(userRequestParams, includeDeleted);
                var userDTOs = _mapper.Map<IList<UserDTO>>(usersWithMetaData);
                return (users: ServiceResponse<IEnumerable<UserDTO>>.SuccessResult(userDTOs, (int)HttpStatusCode.OK, "Users retrieved successfully"), metadata: usersWithMetaData.MetaData);
            }
            catch (Exception ex)
            {
                return (users: ServiceResponse<IEnumerable<UserDTO>>.Failure(
                    "Error while retrieving users",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest), metadata: new MetaData());
            }
        }

        public async Task<ServiceResponse<UserDTO>> GetUserAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUser(id);
                if (user == null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"User with ID {id} not found",
                        (int)HttpStatusCode.BadRequest);
                }
                var userDTO = _mapper.Map<UserDTO>(user);
                return ServiceResponse<UserDTO>.SuccessResult(userDTO, (int)HttpStatusCode.OK, $"User {id} retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserDTO>.Failure(
                    $"Error while retrieving user {id}",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserDTO>> GetUserByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return ServiceResponse<UserDTO>.Failure(
                        "Email cannot be empty",
                        (int)HttpStatusCode.BadRequest);
                }
                var user = await _userRepository.GetUserByEmail(email.ToLower());
                if (user == null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"User with email {email} not found",
                        (int)HttpStatusCode.BadRequest);
                }
                var userDTO = _mapper.Map<UserDTO>(user);
                return ServiceResponse<UserDTO>.SuccessResult(userDTO, (int)HttpStatusCode.OK, $"User with email {email} retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserDTO>.Failure(
                    $"Error while retrieving user with email {email}",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserDTO>> GetUserByUserNameAsync(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return ServiceResponse<UserDTO>.Failure(
                        "Username cannot be empty",
                        (int)HttpStatusCode.BadRequest);
                }
                var user = await _userRepository.GetUserByUserName(userName);
                if (user == null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"User with username {userName} not found",
                        (int)HttpStatusCode.BadRequest);
                }
                var userDTO = _mapper.Map<UserDTO>(user);
                return ServiceResponse<UserDTO>.SuccessResult(userDTO, (int)HttpStatusCode.OK, $"User with username {userName} retrieved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserDTO>.Failure(
                    $"Error while retrieving user with username {userName}",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserDTO>> CreateUserAsync(UserCreateDTO userCreate)
        {
            try
            {
                if (userCreate == null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        "User data cannot be null",
                        (int)HttpStatusCode.BadRequest);
                }

                if (await _userRepository.GetUserByEmail(userCreate.Email.ToLower()) != null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"User with email {userCreate.Email} already exists",
                        (int)HttpStatusCode.BadRequest);
                }
                if (await _userRepository.GetUserByUserName(userCreate.Username) != null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"User with username {userCreate.Username} already exists",
                        (int)HttpStatusCode.BadRequest);
                }

                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = userCreate.Username.Trim(),
                    Email = userCreate.Email.Trim().ToLower(),
                    Password = _passwordHasher.HashPassword(userCreate.Password.Trim()),
                };

                var success = await _userRepository.CreateUser(user);
                if (!success)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        "Failed to create user",
                        (int)HttpStatusCode.BadRequest);
                }

                var userDTO = _mapper.Map<UserDTO>(user);
                await _publishEndpoint.Publish<UserRegistered>(new
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                });
                return ServiceResponse<UserDTO>.SuccessResult(userDTO, (int)HttpStatusCode.OK, "User created successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserDTO>.Failure(
                    "Error while creating user",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserDTO>> UpdateUserAsync(Guid id, UserUpdateDTO userUpdate)
        {
            try
            {
                if (userUpdate == null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        "Update data cannot be null",
                        (int)HttpStatusCode.BadRequest);
                }

                var existingUserByEmail = await _userRepository.GetUserByEmail(userUpdate.Email.ToLower());
                if (existingUserByEmail != null && existingUserByEmail.Id != id)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"Email {userUpdate.Email} is already in use by another user",
                        (int)HttpStatusCode.BadRequest);
                }
                var existingUserByUsername = await _userRepository.GetUserByUserName(userUpdate.Username);
                if (existingUserByUsername != null && existingUserByUsername.Id != id)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"Username {userUpdate.Username} is already in use by another user",
                        (int)HttpStatusCode.BadRequest);
                }

                var updatedUser = await _userRepository.UpdateUserById(id, userUpdate);
                if (updatedUser == null)
                {
                    return ServiceResponse<UserDTO>.Failure(
                        $"User with ID {id} not found or update failed",
                        (int)HttpStatusCode.BadRequest);
                }

                var userDTO = _mapper.Map<UserDTO>(updatedUser);
                return ServiceResponse<UserDTO>.SuccessResult(userDTO, (int)HttpStatusCode.OK, $"User {id} updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserDTO>.Failure(
                    $"Error while updating user {id}",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse> SoftDeleteUserAsync(Guid id)
        {
            try
            {
                var success = await _userRepository.SoftDeleteUserById(id);
                if (!success)
                {
                    return ServiceResponse.Failure(
                        $"User with ID {id} not found or soft delete failed",
                        (int)HttpStatusCode.BadRequest);
                }
                return ServiceResponse.SuccessResult((int)HttpStatusCode.OK, $"User {id} soft deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure(
                    $"Error while soft deleting user {id}",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse> PermanentDeleteUserAsync(Guid id)
        {
            try
            {
                var success = await _userRepository.PermanentDeleteUserById(id);
                if (!success)
                {
                    return ServiceResponse.Failure(
                        $"User with ID {id} not found or permanent delete failed",
                        (int)HttpStatusCode.BadRequest);
                }
                return ServiceResponse.SuccessResult((int)HttpStatusCode.OK, $"User {id} permanently deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure(
                    $"Error while permanently deleting user {id}",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<bool>> IsUserExistAsync(Guid id)
        {
            try
            {
                var exists = await _userRepository.IsUserExist(id);
                return ServiceResponse<bool>.SuccessResult(exists, (int)HttpStatusCode.OK, exists ? $"User {id} exists" : $"User {id} does not exist");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.Failure(
                    $"Error while checking if user {id} exists",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<(ServiceResponse<IEnumerable<UserDTO>> users, MetaData metadata)> GetDeletedUsersAsync(UserRequestParameters userRequestParams)
        {
            try
            {
                var deletedUsersWithMetaData = await _userRepository.GetDeletedUsers(userRequestParams);
                var userDTOs = _mapper.Map<IList<UserDTO>>(deletedUsersWithMetaData);
                return (users: ServiceResponse<IEnumerable<UserDTO>>.SuccessResult(userDTOs, (int)HttpStatusCode.OK, "Deleted users retrieved successfully"), metadata: deletedUsersWithMetaData.MetaData);
            }
            catch (Exception ex)
            {
                return (users: ServiceResponse<IEnumerable<UserDTO>>.Failure(
                    "Error while retrieving deleted users",
                    new List<string> { ex.Message },
                    (int)HttpStatusCode.BadRequest), metadata: new MetaData());
            }
        }

        public async Task<ServiceResponse> SendForgotPasswordOTPByUserNameOrEmail(string userNameOrEmail)
        {
            try
            {
                string email = "";
                userNameOrEmail = userNameOrEmail.Trim();
                var user = await _userRepository.GetUserByEmail(userNameOrEmail);

                if (user == null)
                {
                    user = await _userRepository.GetUserByUserName(userNameOrEmail);
                    if (user == null) return ServiceResponse.Failure("This user is not registered", (int)HttpStatusCode.NotFound);

                    email = user.Email;
                }
                else
                {
                    email = user.Email;
                }

                var otp = OTPGenerator.Generate();
                // gRPC call to the email service for sending the otp

                using var channel = GrpcChannel.ForAddress(_notificationServiceGrpcServer);
                var client = new GrpcNotificationServiceClient(channel);
                var grpcRequest = new SendOtpRequest()
                {
                    Email = email,
                    Username = user.Username,
                    Otp = otp
                };

                var updatedUser = await _userRepository.InsertForgotPasswordOTPandExpirationTime(user.Id, otp);

                if (updatedUser == null)
                {
                    return ServiceResponse.Failure("Error: please try again later", (int)HttpStatusCode.InternalServerError);
                }

                var response = await client.SendOtpAsync(grpcRequest);

                {
                    if (response.Success)
                        return ServiceResponse.SuccessResult((int)HttpStatusCode.OK);
                }

                return ServiceResponse.Failure("Error occoured", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return ServiceResponse.Failure("Error while sending otp", [ex.Message], (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<UserLoginResponse>> ValidateOtp(string userNameOrEmail, string otp)
        {
            try
            {
                var user = await _userRepository.IsValidOtp(userNameOrEmail.Trim(), otp.Trim());
                if (user != null)
                {
                    var (token, tokenExpiryTime) = _tokenProvider.GenerateJwtToken(user.Id, user.Username, user.Role.ToString(), isTokenGeneratedWhileLogin: true);
                    var response = new UserLoginResponse
                    {
                        Id = user.Id,
                        UserName = user.Username,
                        Email = user.Email,
                        Role = user.Role.ToString(),
                        Token = token,
                        ExpiresIn = (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds
                    };
                    return ServiceResponse<UserLoginResponse>.SuccessResult(response, (int)HttpStatusCode.OK);
                }
                return ServiceResponse<UserLoginResponse>.Failure("Invalid OTP", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserLoginResponse>.Failure("Error try again later", [ex.Message], (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<ServiceResponse<UserDTO>> ResetPassword(string password, string? userId, Guid u)
        {

            if (userId == null || !Guid.TryParse(userId, out Guid _id))
            {
                return ServiceResponse<UserDTO>.Failure("Invalid user id", (int)HttpStatusCode.BadRequest);
            }

            try
            {
                var user = await _userRepository.GetUser(u);

                if(user == null)
                {
                    return ServiceResponse<UserDTO>.Failure("User does not exist", (int)HttpStatusCode.BadRequest);
                }

                if(user.Id != _id)
                {
                    return ServiceResponse<UserDTO>.Failure("Unauthorized to perform this operation", (int)HttpStatusCode.Unauthorized);
                }
                var updatedUser = await _userRepository.UpdatePassword(user.Id, _passwordHasher.HashPassword(password.Trim()));

                if(updatedUser == null)
                {
                    ServiceResponse<UserDTO>.Failure("Error while updating new password", (int)HttpStatusCode.InternalServerError);
                }

                return ServiceResponse<UserDTO>.SuccessResult(_mapper.Map<UserDTO>(user), (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserDTO>.Failure("Error while updaing user", [ex.Message], (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}