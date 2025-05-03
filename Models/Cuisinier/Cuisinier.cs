using System;
using LivinParis.Models.Client;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Cuisinier
{
    /// <summary>
    /// Represents a cook in the system.
    /// </summary>
    public class Cuisinier : Utilisateur
    {
        /// <summary>
        /// Gets or sets the cook's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cook's last name.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the cook's first name.
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Gets or sets the cook's specialty.
        /// </summary>
        public string Specialite { get; set; }

        /// <summary>
        /// Gets or sets the cook's years of experience.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Gets or sets the cook's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the cook's phone number.
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Gets or sets the cook's station.
        /// </summary>
        public Station Station { get; set; }

        public Cuisinier(string nom, string prenom, Station station, string telephone, string email)
            : base()
        {
            Nom = nom;
            Prenom = prenom;
            Station = station;
            Telephone = telephone;
            Email = email;
        }

        /// <summary>
        /// Returns a string representation of the cook.
        /// </summary>
        /// <returns>A string containing the cook's name and specialty.</returns>
        public override string ToString()
        {
            return $"{Nom} {Prenom} - {Specialite}";
        }
    }
} 