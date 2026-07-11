using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Services;

namespace Backend_session_10_SoC.Presentations
{
    public class UserPresentation
    {
        private UserService userService;

        public UserPresentation(UserService userService)
        {
            this.userService = userService;
        }

        public void RegisterUser()
        {
            Console.WriteLine("\n=== Register New User ===");

            Console.Write("Enter username: ");
            string name = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Console.Write("Enter full name: ");
            string fullName = Console.ReadLine();

            Console.Write("Enter phone number (optional — Enter to skip): ");
            string phone = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(phone)) phone = null;

            Console.Write("Enter address (optional — Enter to skip): ");
            string address = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(address)) address = null;

            userService.Register(name, email, password, fullName, phone, address);

            int newId = userService.GetLastUserId();
            Console.WriteLine($"User registered successfully. Assigned ID: {newId}");
        }
    }
}
