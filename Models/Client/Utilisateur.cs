using System;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Client
{
    /// <summary>
    /// Represents a base user in the system.
    /// </summary>
    public abstract class Utilisateur
    {
        private string _motDePasse;
        
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

        public Station Station { get; set; }
        public string Password { get; private set; }
        public decimal MontantAchats { get; protected set; }
        public bool EstEntreprise { get; set; }

        /// <summary>
        /// Initializes a new instance of the Utilisateur class.
        /// </summary>
        protected Utilisateur()
        {
            Identifiant = GenererIdentifiantUnique();
            DateCreation = DateTime.Now;
            MontantAchats = 0;
        }

        protected Utilisateur(string nom, string prenom, Station station, string telephone, string email)
            : this()
        {
            Station = station ?? throw new ArgumentNullException(nameof(station));
            Telephone = telephone;
            Email = email;
            Nom = nom;
            Prenom = prenom;
        }

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public bool VerifierMotDePasse(string motDePasse)
        {
            if (string.IsNullOrEmpty(motDePasse))
                return false;
            return _motDePasse == motDePasse;
        }

        public void DefinirMotDePasse(string motDePasse)
        {
            if (string.IsNullOrEmpty(motDePasse))
                throw new ArgumentException("Le mot de passe ne peut pas être vide.");
            _motDePasse = motDePasse;
            Password = motDePasse; // For compatibility with existing code
        }

        private string GenererIdentifiantUnique()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
} 