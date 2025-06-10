using RequestFeatureShared;
using ServiceResponseShared;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.RequestFeatures;

namespace UserAuthenticationService.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDTO>> CreateUserAsync(UserCreateDTO userCreate);
        Task<(ServiceResponse<IEnumerable<UserDTO>> users, MetaData metadata)> GetDeletedUsersAsync(UserRequestParameters userRequestParams);
        Task<ServiceResponse<UserDTO>> GetUserAsync(Guid id);
        Task<ServiceResponse<UserDTO>> GetUserByEmailAsync(string email);
        Task<ServiceResponse<UserDTO>> GetUserByUserNameAsync(string userName);
        Task<(ServiceResponse<IEnumerable<UserDTO>> users, MetaData metadata)> GetUsersAsync(UserRequestParameters userRequestParams, bool includeDeleted = false);
        Task<ServiceResponse<bool>> IsUserExistAsync(Guid id);
        Task<ServiceResponse> PermanentDeleteUserAsync(Guid id);
        Task<ServiceResponse> SoftDeleteUserAsync(Guid id);
        Task<ServiceResponse<UserDTO>> UpdateUserAsync(Guid id, UserUpdateDTO userUpdate);
    }
}