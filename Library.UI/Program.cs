using Library.Aplication.Services;
using Library.Infrastructure.Repositories;

namespace Library.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserRepository repository = new UserRepository();
            UserService service = new UserService(repository);

            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");

            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                Console.WriteLine("Enter name:");
                string name = Console.ReadLine() ?? "";

                Console.WriteLine("Enter email:");
                string email = Console.ReadLine() ?? "";

                Console.WriteLine("Enter password:");
                string password = Console.ReadLine() ?? "";

                service.RegisterUser(name, email, password);

                Console.WriteLine("User registered successfully.");
            }
            else if (choice == "2")
            {
                Console.WriteLine("Enter email:");
                string email = Console.ReadLine() ?? "";

                Console.WriteLine("Enter password:");
                string password = Console.ReadLine() ?? "";

                service.LoginUser(email, password);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
    }
}