namespace LivinParis.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        public UserRole Role { get; set; }

        public User(string username, string password, UserRole role, string email)
        {
            Username = username;
            Password = password;
            Role = role;
            Email = email;
        }
    }

    /// <summary>
    /// Defines the possible roles for users in the system.
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Administrator role with full system access.
        /// </summary>
        Admin,

        /// <summary>
        /// Manager role with elevated privileges.
        /// </summary>
        Manager,

        /// <summary>
        /// Standard user role with basic access.
        /// </summary>
        User
    }
} 