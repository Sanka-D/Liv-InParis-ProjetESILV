using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Commande
{
    public enum StatutCommande
    {
        EnAttente,
        EnPreparation,
        EnLivraison,
        Livree,
        Terminee,
        Annulee
    }

    public class Commande
    {
        public string Identifiant { get; private set; }
        public string IdentifiantClient { get; private set; }
        public string IdentifiantCuisinier { get; set; }
        public DateTime DateCommande { get; set; }
        public string AdresseLivraison { get; set; }
        public List<LigneCommande> LignesCommande { get; private set; }
        public StatutCommande Statut { get; set; }
        public decimal MontantTotal => LignesCommande.Sum(l => l.SousTotal);
        public Station Station { get; private set; }
        public Models.Client.Client Client { get; private set; }
        public Models.Cuisinier.Cuisinier Cuisinier { get; private set; }
        public DateTime Date { get; set; }

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

        public void AjouterLigne(string nomPlat, int quantite, decimal prixUnitaire)
        {
            LignesCommande.Add(new LigneCommande(nomPlat, quantite, prixUnitaire));
        }

        public void ModifierLigne(int index, int nouvelleQuantite)
        {
            if (index >= 0 && index < LignesCommande.Count)
            {
                LignesCommande[index].Quantite = nouvelleQuantite;
            }
        }

        public void SupprimerLigne(int index)
        {
            if (index < 0 || index >= LignesCommande.Count)
                throw new ArgumentException("Index de ligne invalide");

            LignesCommande.RemoveAt(index);
        }

        public void DefinirClient(Models.Client.Client client)
        {
            Client = client;
        }

        public void DefinirCuisinier(Models.Cuisinier.Cuisinier cuisinier)
        {
            Cuisinier = cuisinier;
        }
    }

    public class LigneCommande
    {
        public string NomPlat { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal SousTotal { get; private set; }

        public LigneCommande(string nomPlat, int quantite, decimal prixUnitaire)
        {
            NomPlat = nomPlat;
            Quantite = quantite;
            PrixUnitaire = prixUnitaire;
            CalculerSousTotal();
        }

        public void CalculerSousTotal()
        {
            SousTotal = Quantite * PrixUnitaire;
        }
    }
} 