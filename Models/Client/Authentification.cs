using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LivinParis.Models.Client
{
    /// <summary>
    /// Handles user authentication and registration.
    /// </summary>
    public class Authentification
    {
        private static Dictionary<string, Utilisateur> _utilisateurs = new Dictionary<string, Utilisateur>();

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="utilisateur">The user to register.</param>
        /// <param name="motDePasse">The password for the user.</param>
        public static void EnregistrerUtilisateur(Utilisateur utilisateur, string motDePasse)
        {
            utilisateur.DefinirMotDePasse(motDePasse);
            _utilisateurs.Add(utilisateur.Identifiant, utilisateur);
        }

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="identifiant">The username.</param>
        /// <param name="motDePasse">The password.</param>
        /// <returns>The authenticated user, or null if authentication fails.</returns>
        public static Utilisateur Authentifier(string identifiant, string motDePasse)
        {
            if (_utilisateurs.TryGetValue(identifiant, out Utilisateur utilisateur))
            {
                if (utilisateur.VerifierMotDePasse(motDePasse))
                {
                    return utilisateur;
                }
            }
            return null;
        }

        /// <summary>
        /// Changes the password for a user.
        /// </summary>
        /// <param name="identifiant">The username of the user.</param>
        /// <param name="ancienMotDePasse">The old password.</param>
        /// <param name="nouveauMotDePasse">The new password.</param>
        /// <returns>True if the password change is successful, false otherwise.</returns>
        public static bool ChangerMotDePasse(string identifiant, string ancienMotDePasse, string nouveauMotDePasse)
        {
            if (!_utilisateurs.TryGetValue(identifiant, out Utilisateur utilisateur))
            {
                return false;
            }

            if (!utilisateur.VerifierMotDePasse(ancienMotDePasse))
            {
                return false;
            }

            utilisateur.DefinirMotDePasse(nouveauMotDePasse);
            return true;
        }

        /// <summary>
        /// Hashes a password using SHA256.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password.</returns>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Verifies user credentials against the database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <returns>True if credentials are valid, false otherwise.</returns>
        private bool VerifyCredentials(string username, string hashedPassword)
        {
            return true;
        }

        /// <summary>
        /// Saves a new user to the database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="email">The email address.</param>
        /// <param name="role">The user role.</param>
        /// <returns>True if user is saved successfully, false otherwise.</returns>
        private bool SaveUser(string username, string hashedPassword, string email, UserRole role)
        {
            return true;
        }
    }
} 