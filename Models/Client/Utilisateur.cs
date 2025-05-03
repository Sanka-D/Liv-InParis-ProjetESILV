using System;

namespace LivinParis.Models.Client
{
    /// <summary>
    /// Represents a base user in the system.
    /// </summary>
    public abstract class Utilisateur
    {
        /// <summary>
        /// Gets or sets the user's unique identifier.
        /// </summary>
        public string Identifiant { get; set; }

        /// <summary>
        /// Gets or sets the user's creation date.
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Gets or sets the user's last login date.
        /// </summary>
        public DateTime? DerniereConnexion { get; set; }

        /// <summary>
        /// Initializes a new instance of the Utilisateur class.
        /// </summary>
        protected Utilisateur()
        {
            Identifiant = Guid.NewGuid().ToString();
            DateCreation = DateTime.Now;
        }

        public bool VerifierMotDePasse(string motDePasse)
        {
            return _motDePasse == motDePasse;
        }

        public void DefinirMotDePasse(string motDePasse)
        {
            _motDePasse = motDePasse;
        }

        private string GenererIdentifiantUnique()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
} 