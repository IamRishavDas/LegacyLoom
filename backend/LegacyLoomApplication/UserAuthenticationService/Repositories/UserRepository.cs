using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using UserAuthenticationService.Data;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Models;

namespace UserAuthenticationService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<bool> CreateUser(User user)
        {
            await _userDbContext.Users.AddAsync(user);
            return await _userDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> PermanentDeleteUserById(Guid id)
        {
            var user = await GetUser(id);
            if (user == null) return false;
            _userDbContext.Users.Remove(user);
            return await _userDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteUserById(Guid id)
        {
            var user = await GetUser(id);
            if (user == null) return false;

            user.IsDeleted = true;
            _userDbContext.Users.Update(user);

            return await _userDbContext.SaveChangesAsync() > 0;
        }

        public async Task<User?> GetUser(Guid id)
        {
            var user = await _userDbContext.Users.FindAsync(id);
            if (user == null || user.IsDeleted) return null;
            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userDbContext.Users.Where(user => user.Email == email && !user.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _userDbContext.Users.Where(user => user.Username == userName && !user.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsers(bool includeDeleted = false)
        {
            return !includeDeleted ? await _userDbContext.Users.Where(user => !user.IsDeleted).ToListAsync() :
                await _userDbContext.Users.ToListAsync();
        }

        public async Task<User?> UpdateUserById(Guid id, UserUpdateDTO userUpdate)
        {
            var user = await GetUser(id);
            if (user == null) return null;

            user.Username = userUpdate.Username;
            user.Email = userUpdate.Email;

            _userDbContext.Users.Update(user);
            return await _userDbContext.SaveChangesAsync() > 0 ? user : null;
        }

        public async Task<bool> IsUserExist(Guid id)
        {
            return await _userDbContext.Users.AnyAsync(user => user.Id == id && !user.IsDeleted);
        }

        public async Task<List<User>> GetDeletedUsers()
        {
            return await _userDbContext.Users.Where(user => user.IsDeleted).ToListAsync();
        }
    }
}
