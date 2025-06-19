
namespace LegacyLoom.Tests
{
    public class UserRepository
    {
        public static IQueryable<User> Users => new List<User>() 
            {
                new(){Id = Guid.NewGuid(), Username = "iamrishav", Email = "rishav@gmail.com", Password = "Rishav@1234", Role = Role.Admin},
                new(){Id = Guid.NewGuid(), Username = "iamrohit", Email = "rohit@gmail.com", Password = "Rohit@1234", Role = Role.User},
                new(){Id = Guid.NewGuid(), Username = "iamsusu", Email = "susu@gmail.com", Password = "Susu@1234", Role = Role.User},
            }.AsQueryable<User>();
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var users = UserRepository.Users;
            var sortedUsers = new Sorting<User>().ApplySort(users, Console.ReadLine());
            foreach(var user in sortedUsers.ToList())
                Console.Write($"{user} ");
        }
    }
}
