using AutoMapper;
using EventModelsShared;
using MassTransit;
using RequestFeatureShared;
using ServiceResponseShared;
using System.Net;
using System.Threading.Tasks;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Models;
using UserAuthenticationService.Repositories;
using UserAuthenticationService.RequestFeatures;
using UserAuthenticationService.Utils;

namespace UserAuthenticationService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, PasswordHasher passwordHasher, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
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
    }
}