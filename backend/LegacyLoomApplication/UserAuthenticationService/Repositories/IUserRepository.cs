using RequestFeatureShared;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Models;
using UserAuthenticationService.RequestFeatures;

namespace UserAuthenticationService.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user);
        Task<PagedList<User>> GetDeletedUsers(UserRequestParameters userRequestParams);
        Task<User?> GetUser(Guid id);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByUserName(string userName);
        Task<PagedList<User>> GetUsers(UserRequestParameters userRequestParams, bool includeDeleted = false);
        Task<bool> IsUserExist(Guid id);
        Task<bool> PermanentDeleteUserById(Guid id);
        Task<bool> SoftDeleteUserById(Guid id);
        Task<User?> UpdateUserById(Guid id, UserUpdateDTO userUpdate);
    }
}
