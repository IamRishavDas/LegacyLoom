using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Models;

namespace UserAuthenticationService.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user);
        Task<List<User>> GetDeletedUsers();
        Task<User?> GetUser(Guid id);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByUserName(string userName);
        Task<List<User>> GetUsers(bool includeDeleted = false);
        Task<bool> IsUserExist(Guid id);
        Task<bool> PermanentDeleteUserById(Guid id);
        Task<bool> SoftDeleteUserById(Guid id);
        Task<User?> UpdateUserById(Guid id, UserUpdateDTO userUpdate);
    }
}
