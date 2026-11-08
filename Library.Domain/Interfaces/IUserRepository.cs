using Library.Domain.Models;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository
    {
        void SaveUser(User user);
        List<User> GetAllUsers();
        User? GetUserByEmail(string email);
    }
}