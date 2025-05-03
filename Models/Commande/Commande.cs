using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Commande
{
    /// <summary>
    /// Represents the status of an order.
    /// </summary>
    public enum StatutCommande
    {
        /// <summary>
        /// Order is pending.
        /// </summary>
        EnAttente,

        /// <summary>
        /// Order is being prepared.
        /// </summary>
        EnPreparation,

        /// <summary>
        /// Order is ready for delivery.
        /// </summary>
        PretPourLivraison,

        /// <summary>
        /// Order is being delivered.
        /// </summary>
        EnLivraison,

        /// <summary>
        /// Order has been delivered.
        /// </summary>
        Livree,

        /// <summary>
        /// Order has been canceled.
        /// </summary>
        Annulee
    }

    /// <summary>
    /// Represents an order in the system.
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Gets or sets the order ID.
        /// </summary>
        public string Identifiant { get; private set; }

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public string IdentifiantClient { get; private set; }

        /// <summary>
        /// Gets or sets the dish ID.
        /// </summary>
        public string IdentifiantCuisinier { get; set; }

        /// <summary>
        /// Gets or sets the quantity ordered.
        /// </summary>
        public DateTime DateCommande { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        public string AdresseLivraison { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        public StatutCommande Statut { get; set; }

        /// <summary>
        /// Gets or sets the total price of the order.
        /// </summary>
        public decimal MontantTotal => LignesCommande.Sum(l => l.SousTotal);

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        public Station Station { get; private set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        public Models.Client.Client Client { get; private set; }

        /// <summary>
        /// Gets or sets the dish.
        /// </summary>
        public Models.Cuisinier.Cuisinier Cuisinier { get; private set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the list of order lines.
        /// </summary>
        public List<LigneCommande> LignesCommande { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Commande class.
        /// </summary>
        public Commande(string identifiantClient, Station station)
        {
            Identifiant = Guid.NewGuid().ToString().Substring(0, 8);
            IdentifiantClient = identifiantClient;
            DateCommande = DateTime.Now;
            Date = DateTime.Now;
            Statut = StatutCommande.EnAttente;
            LignesCommande = new List<LigneCommande>();
            Station = station;
        }

        /// <summary>
        /// Adds a line to the order.
        /// </summary>
        /// <param name="nomPlat">The name of the dish.</param>
        /// <param name="quantite">The quantity of the dish.</param>
        /// <param name="prixUnitaire">The price of one dish.</param>
        public void AjouterLigne(string nomPlat, int quantite, decimal prixUnitaire)
        {
            LignesCommande.Add(new LigneCommande(nomPlat, quantite, prixUnitaire));
        }

        /// <summary>
        /// Modifies a line in the order.
        /// </summary>
        /// <param name="index">The index of the line to modify.</param>
        /// <param name="nouvelleQuantite">The new quantity for the line.</param>
        public void ModifierLigne(int index, int nouvelleQuantite)
        {
            if (index >= 0 && index < LignesCommande.Count)
            {
                LignesCommande[index].Quantite = nouvelleQuantite;
            }
        }

        /// <summary>
        /// Removes a line from the order.
        /// </summary>
        /// <param name="index">The index of the line to remove.</param>
        public void SupprimerLigne(int index)
        {
            if (index < 0 || index >= LignesCommande.Count)
                throw new ArgumentException("Index de ligne invalide");

            LignesCommande.RemoveAt(index);
        }

        /// <summary>
        /// Defines the client for the order.
        /// </summary>
        /// <param name="client">The client for the order.</param>
        public void DefinirClient(Models.Client.Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Defines the dish for the order.
        /// </summary>
        /// <param name="cuisinier">The dish for the order.</param>
        public void DefinirCuisinier(Models.Cuisinier.Cuisinier cuisinier)
        {
            Cuisinier = cuisinier;
        }

        /// <summary>
        /// Returns a string representation of the order.
        /// </summary>
        /// <returns>A string containing the order details.</returns>
        public override string ToString()
        {
            return $"Commande #{Identifiant} - Client #{IdentifiantClient} - {Statut} - {MontantTotal:C}";
        }
    }

    /// <summary>
    /// Represents a line in an order.
    /// </summary>
    public class LigneCommande
    {
        /// <summary>
        /// Gets or sets the name of the dish.
        /// </summary>
        public string NomPlat { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the dish.
        /// </summary>
        public int Quantite { get; set; }

        /// <summary>
        /// Gets or sets the price of one dish.
        /// </summary>
        public decimal PrixUnitaire { get; set; }

        /// <summary>
        /// Gets or sets the total price of the line.
        /// </summary>
        public decimal SousTotal { get; private set; }

        /// <summary>
        /// Initializes a new instance of the LigneCommande class.
        /// </summary>
        /// <param name="nomPlat">The name of the dish.</param>
        /// <param name="quantite">The quantity of the dish.</param>
        /// <param name="prixUnitaire">The price of one dish.</param>
        public LigneCommande(string nomPlat, int quantite, decimal prixUnitaire)
        {
            NomPlat = nomPlat;
            Quantite = quantite;
            PrixUnitaire = prixUnitaire;
            CalculerSousTotal();
        }

        /// <summary>
        /// Calculates the total price of the line.
        /// </summary>
        public void CalculerSousTotal()
        {
            SousTotal = Quantite * PrixUnitaire;
        }
    }
} 