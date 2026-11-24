using Library.Domain.Models;
using Library.Domain.Interfaces;
using System.Security.Cryptography;


namespace Library.Aplication.Services
{
    public class UserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public void RegisterUser(string name, string email, string password, string role)
        {
            var users = _userRepository.GetAllUsers();

            var nextId = users.Count == 0 ? 1 : users.Max(user => user.Id) + 1;
            var verificationCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            var user = new User
            {
                Id = nextId,
                Name = name,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                IsVerified = false,
                VerificationCode = verificationCode,
                Role = role,

            };
            _userRepository.SaveUser(user);


            _emailService.SendEmail(email, "Verification Code", $"Your verification code is: {verificationCode}");
        }

        public User? LoginUser(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public bool SendLoginVerificationCode(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            _emailService.SendEmail(email, "Login Code", $"Your login code is: {user.VerificationCode}");
            return true;
        }

        public bool VerifyLoginCode(string email, string verificationCode)
        {
            var user = _userRepository.GetUserByEmail(email);
            return user != null && user.VerificationCode == verificationCode;
        }

        public bool VerifyUser(string email, string verificationCode)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null || user.VerificationCode != verificationCode)
            {
                return false;
            }

            user.IsVerified = true;
            _userRepository.SaveUser(user);
            return true;
        }
    }
}