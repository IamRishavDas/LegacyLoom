using Microsoft.EntityFrameworkCore;
using RequestFeatureShared;
using RequestFeatureShared.SortHelper;
using UserAuthenticationService.Data;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Models;
using UserAuthenticationService.RequestFeatures;

namespace UserAuthenticationService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;
        private readonly ISortHelper<User> _userSortHelper;

        public UserRepository(UserDbContext userDbContext, ISortHelper<User> userSortHelper)
        {
            _userDbContext = userDbContext;
            _userSortHelper = userSortHelper;
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

        public async Task<PagedList<User>> GetUsers(UserRequestParameters userRequestParams, bool includeDeleted = false)
        {
            var results = !includeDeleted ? _userDbContext.Users
                .Where(user => !user.IsDeleted)
                : _userDbContext.Users;

            results = SearchByName(results, userRequestParams.Name);
            var sortedUsers = _userSortHelper.ApplySort(results, userRequestParams.OrderBy);

            var count = await results.CountAsync();
            var users = await results
                .Skip((userRequestParams.PageNumber - 1) * userRequestParams.PageSize).Take(userRequestParams.PageSize)
                .ToListAsync();
            return PagedList<User>.ToPagedList(sortedUsers, count, userRequestParams.PageNumber, userRequestParams.PageSize);
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

        public async Task<PagedList<User>> GetDeletedUsers(UserRequestParameters userRequestParams)
        {
            var results = _userDbContext.Users.Where(user => user.IsDeleted);

            results = SearchByName(results, userRequestParams.Name);
            var count = await results.CountAsync();
            var users = await results
                .Skip((userRequestParams.PageNumber - 1) * userRequestParams.PageSize).Take(userRequestParams.PageSize)
                .ToListAsync();
            return PagedList<User>.ToPagedList(users, count, userRequestParams.PageNumber, userRequestParams.PageSize);

        }

        private IQueryable<User> SearchByName(IQueryable<User> users, string? searchName)
        {
            if (!users.Any() || string.IsNullOrWhiteSpace(searchName)) return users;
            var searchedUsers = users.Where(user => user.Username.ToLower().Contains(searchName.Trim().ToLower()));
            return searchedUsers;
        }

        public async Task<User?> InsertForgotPasswordOTPandExpirationTime(Guid userId, string otp)
        {
            var user = await _userDbContext.Users.FindAsync(userId);

            if (user == null) return null;

            user.LastForgotPasswordOTP = otp;
            user.OTPExpirationTime = DateTime.UtcNow.AddMinutes(5);

            _userDbContext.Users.Update(user);
            return (await _userDbContext.SaveChangesAsync()) > 0 ? user : null ;
        }

        public async Task<User?> IsValidOtp(string userNameOrEmail, string otp)
        {
            var user = await GetUserByEmail(userNameOrEmail) ?? await GetUserByUserName(userNameOrEmail);
            if (user == null) return null;

            if (user.LastForgotPasswordOTP == otp && user.OTPExpirationTime >= DateTime.UtcNow) return user;
            return null;
        }

        public async Task<User?> UpdatePassword(Guid userId, string password)
        {
            var user = await GetUser(userId);
            if(user == null)
            {
                return null;
            }

            user.Password = password;
            _userDbContext.Users.Update(user);
            return await _userDbContext.SaveChangesAsync() > 0 ? user : null;
        }
    }
}
