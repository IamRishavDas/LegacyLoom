using UserAuthenticationService.Models;

namespace UserAuthenticationService.Repositories
{
    public class UserRepository : IUserRepository
    {

        private static List<User> _users = new List<User>()
            {
                new(){Id = Guid.NewGuid(), Email = "iamrishavdas@gmail.com", Password = "rishav", Username = "iamrishavdas", Role = Enums.Role.Admin},
                new(){Id = Guid.NewGuid(), Email = "iamrohit@gmail.com", Password = "rohit", Username = "iamrohit", Role = Enums.Role.Moderator},
                new(){Id = Guid.NewGuid(), Email = "iammanish@gmail.com", Password = "manish", Username = "manish", Role = Enums.Role.User},
            };


        public User CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(Guid id)
        {
            return _users.Find(user => user.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            return _users.Find(user => user.Email == email);
        }

        public User GetUserByUserName(string userName)
        {
            return _users.Find(user => user.Username == userName);
        }

        public List<User> GetUsers(bool includeDeleted = false)
        {
            return _users;
        }

        public User UpdateUserById(Guid id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
