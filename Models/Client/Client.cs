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
        /// Gets or sets the client's enterprise name.
        /// </summary>
        public string NomEntreprise { get; set; }

        /// <summary>
        /// Gets or sets the client's enterprise referent.
        /// </summary>
        public string ReferentEntreprise { get; set; }

        /// <summary>
        /// Gets or sets whether the client is an enterprise.
        /// </summary>
        public bool EstEntreprise { get; set; }

        /// <summary>
        /// Gets or sets the client's enterprise number.
        /// </summary>
        public string NumeroSiret { get; set; }

        /// <summary>
        /// Initializes a new instance of the Client class.
        /// </summary>
        public Client()
            : base()
        {
            Commandes = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the Client class with specific parameters.
        /// </summary>
        /// <param name="nom">The client's last name.</param>
        /// <param name="prenom">The client's first name.</param>
        /// <param name="station">The client's station.</param>
        /// <param name="telephone">The client's phone number.</param>
        /// <param name="email">The client's email address.</param>
        public Client(string nom, string prenom, Station station, string telephone, string email)
            : base(nom, prenom, station, telephone, email)
        {
            Commandes = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the Client class with a specific ID and name.
        /// </summary>
        /// <param name="id">The client's ID.</param>
        /// <param name="nom">The client's last name.</param>
        /// <param name="prenom">The client's first name.</param>
        public Client(int id, string nom, string prenom)
            : base()
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Commandes = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the Client class for enterprise clients.
        /// </summary>
        /// <param name="nomEntreprise">The client's enterprise name.</param>
        /// <param name="referentEntreprise">The client's enterprise referent.</param>
        /// <param name="numeroSiret">The client's enterprise number.</param>
        /// <param name="station">The client's station.</param>
        /// <param name="telephone">The client's phone number.</param>
        /// <param name="email">The client's email address.</param>
        public Client(string nomEntreprise, string referentEntreprise, string numeroSiret, Station station, string telephone, string email)
            : base()
        {
            NomEntreprise = nomEntreprise;
            ReferentEntreprise = referentEntreprise;
            NumeroSiret = numeroSiret;
            EstEntreprise = true;
            Station = station;
            Telephone = telephone;
            Email = email;
            Commandes = new List<int>();
        }

        /// <summary>
        /// Returns a string representation of the client.
        /// </summary>
        /// <returns>A string containing the client's name and contact information.</returns>
        public override string ToString()
        {
            if (EstEntreprise)
                return $"{NomEntreprise} (Référent: {ReferentEntreprise}) - {Email} - {Telephone}";
            return $"{Nom} {Prenom} - {Email} - {Telephone}";
        }

        public void AjouterAchat(decimal montant)
        {
            if (montant < 0)
                throw new ArgumentException("Le montant ne peut pas être négatif");
                
            MontantAchats += montant;
        }

        public void AjouterCommande(int commandeId)
        {
            if (!Commandes.Contains(commandeId))
            {
                Commandes.Add(commandeId);
            }
        }
    }
} 