using System;
using System.Collections.Generic;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Models.Commande
{
    public enum StatutCommande
    {
        EnAttente,
        EnPreparation,
        EnLivraison,
        Livree,
        Annulee
    }

    public class Commande
    {
        public string Identifiant { get; private set; }
        public string IdentifiantClient { get; set; }
        public string IdentifiantCuisinier { get; set; }
        public DateTime DateCommande { get; set; }
        public string AdresseLivraison { get; set; }
        public List<LigneCommande> LignesCommande { get; set; }
        public StatutCommande Statut { get; set; }
        public decimal MontantTotal { get; private set; }

        public Commande(string identifiantClient, string adresseLivraison)
        {
            Identifiant = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            IdentifiantClient = identifiantClient;
            DateCommande = DateTime.Now;
            AdresseLivraison = adresseLivraison;
            LignesCommande = new List<LigneCommande>();
            Statut = StatutCommande.EnAttente;
            MontantTotal = 0;
        }

        public void AjouterLigne(string nomPlat, int quantite, decimal prixUnitaire)
        {
            var ligne = new LigneCommande(nomPlat, quantite, prixUnitaire);
            LignesCommande.Add(ligne);
            CalculerMontantTotal();
        }

        public void ModifierLigne(int index, int nouvelleQuantite)
        {
            if (index < 0 || index >= LignesCommande.Count)
                throw new ArgumentException("Index de ligne invalide");

            LignesCommande[index].Quantite = nouvelleQuantite;
            LignesCommande[index].CalculerSousTotal();
            CalculerMontantTotal();
        }

        public void SupprimerLigne(int index)
        {
            if (index < 0 || index >= LignesCommande.Count)
                throw new ArgumentException("Index de ligne invalide");

            LignesCommande.RemoveAt(index);
            CalculerMontantTotal();
        }

        private void CalculerMontantTotal()
        {
            MontantTotal = 0;
            foreach (var ligne in LignesCommande)
            {
                MontantTotal += ligne.SousTotal;
            }
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