using Library.Domain.Models;
using Library.Domain.Interfaces;
using System.IO;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Library",
            "Data",
            "UserData.txt"
        );

        public void SaveUser(User user)
        {
            string directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.AppendAllText(_path, user.ToString() + Environment.NewLine);
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            if (!File.Exists(_path))
            {
                return users;
            }

            string[] lines = File.ReadAllLines(_path);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');

                if (parts.Length == 6)
                {
                    User user = new User();

                    user.Id = int.Parse(parts[0]);
                    user.Name = parts[1];
                    user.Email = parts[2];
                    user.Password = parts[3];
                    user.VerifiCationCode = parts[4];
                    user.IsVerified = bool.Parse(parts[5]);

                    users.Add(user);
                }
            }

            return users;
        }

        public User? GetUserByEmail(string email)
        {
            List<User> users = GetAllUsers();

            foreach (User user in users)
            {
                if (user.Email.ToLower() == email.ToLower())
                {
                    return user;
                }
            }

            return null;
        }
    }
}