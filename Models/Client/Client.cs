using System;
using System.Collections.Generic;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Client
{
    /// <summary>
    /// Represents a client in the system.
    /// </summary>
    public class Client : Utilisateur
    {
        /// <summary>
        /// Gets or sets the client's ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the client's last name.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the client's first name.
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Gets or sets the client's address.
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// Gets or sets the client's phone number.
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Gets or sets the client's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the client's dietary preferences.
        /// </summary>
        public string Preferences { get; set; }

        /// <summary>
        /// Gets or sets the list of orders made by the client.
        /// </summary>
        public List<int> Commandes { get; set; }

        /// <summary>
        /// Initializes a new instance of the Client class.
        /// </summary>
        public Client()
        {
            Commandes = new List<int>();
        }

        /// <summary>
        /// Returns a string representation of the client.
        /// </summary>
        /// <returns>A string containing the client's name and contact information.</returns>
        public override string ToString()
        {
            return $"{Nom} {Prenom} - {Email} - {Telephone}";
        }

        public void AjouterAchat(decimal montant)
        {
            if (montant < 0)
                throw new ArgumentException("Le montant ne peut pas être négatif");
                
            MontantAchats += montant;
        }
    }
} 