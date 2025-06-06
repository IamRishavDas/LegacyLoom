using UserAuthenticationService.Models;

namespace UserAuthenticationService.Repositories
{
    public interface IUserRepository
    {
        public List<User> GetUsers(bool includeDeleted = false);
        public User CreateUser(User user);
        public User GetUser(Guid id);
        public User GetUserByUserName(string userName);
        public User GetUserByEmail(string email);
        public User UpdateUserById(Guid id, User user);
        public bool DeleteUserById(Guid id);
    }
}
