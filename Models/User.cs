namespace LivinParis.Models
{
    public enum UserRole
    {
        Admin,
        Manager,
        User
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }

        public User(string username, string password, UserRole role, string email)
        {
            Username = username;
            Password = password;
            Role = role;
            Email = email;
        }
    }
} 