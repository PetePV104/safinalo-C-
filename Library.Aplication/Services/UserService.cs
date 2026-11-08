using Library.Domain.Models;
using Library.Domain.Interfaces;

namespace Library.Aplication.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(string name, string email, string password)
        {
            List<User> users = _userRepository.GetAllUsers();

            int newId = 1;

            foreach (User user in users)
            {
                if (user.Id >= newId)
                {
                    newId = user.Id + 1;
                }
            }

            User newUser = new User();

            newUser.Id = newId;
            newUser.Name = name;
            newUser.Email = email;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
            newUser.IsVerified = false;

            _userRepository.SaveUser(newUser);
        }

        public void LoginUser(string email, string password)
        {
            User? user = _userRepository.GetUserByEmail(email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
            }
        }
    }
}