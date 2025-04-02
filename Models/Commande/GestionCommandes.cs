using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Commande
{
    public static class GestionCommandes
    {
        private static List<Commande> _commandes = new List<Commande>();
        private static readonly string _fichierCommandes = "commandes.json";

        public static void Initialiser()
        {
            ChargerCommandes();
        }

        private static void ChargerCommandes()
        {
            try
            {
                if (File.Exists(_fichierCommandes))
                {
                    var json = File.ReadAllText(_fichierCommandes);
                    _commandes = JsonSerializer.Deserialize<List<Commande>>(json) ?? new List<Commande>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des commandes : {ex.Message}");
                _commandes = new List<Commande>();
            }
        }

        private static void SauvegarderCommandes()
        {
            try
            {
                var json = JsonSerializer.Serialize(_commandes);
                File.WriteAllText(_fichierCommandes, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde des commandes : {ex.Message}");
            }
        }

        public static void ReinitialiserCommandes()
        {
            _commandes.Clear();
            SauvegarderCommandes();
        }

        // Création et modification de commandes
        public static Commande CreerCommande(Models.Client.Client client, Models.Cuisinier.Cuisinier cuisinier, DateTime date)
        {
            if (client == null || cuisinier == null)
                throw new Exception("Le client et le cuisinier sont requis");

            var commande = new Commande(client.Identifiant, client.Station);
            commande.IdentifiantCuisinier = cuisinier.Identifiant;
            commande.DateCommande = date;
            commande.Date = date;
            commande.DefinirClient(client);
            commande.DefinirCuisinier(cuisinier);
            _commandes.Add(commande);
            SauvegarderCommandes();
            return commande;
        }

        public static void ModifierCommande(string identifiant, Commande nouvelleCommande)
        {
            var commande = TrouverCommande(identifiant);
            commande.IdentifiantCuisinier = nouvelleCommande.IdentifiantCuisinier;
            commande.Statut = nouvelleCommande.Statut;
            SauvegarderCommandes();
        }

        public static void SupprimerCommande(string identifiant)
        {
            var commande = TrouverCommande(identifiant);
            _commandes.Remove(commande);
            SauvegarderCommandes();
        }

        // Gestion du cycle de vie des commandes
        public static void AssignerCuisinier(string identifiantCommande, string identifiantCuisinier)
        {
            var commande = TrouverCommande(identifiantCommande);
            var cuisinier = GestionCuisiniers.ObtenirCuisinier(identifiantCuisinier);
            if (cuisinier == null)
                throw new Exception("Cuisinier non trouvé");

            commande.IdentifiantCuisinier = cuisinier.Identifiant;
            commande.DefinirCuisinier(cuisinier);
            commande.Statut = StatutCommande.EnPreparation;
            SauvegarderCommandes();
        }

        public static void DemarrerPreparation(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            commande.Statut = StatutCommande.EnPreparation;
            SauvegarderCommandes();
        }

        public static void DemarrerLivraison(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            commande.Statut = StatutCommande.EnLivraison;
            SauvegarderCommandes();
        }

        public static void TerminerCommande(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            commande.Statut = StatutCommande.Livree;
            SauvegarderCommandes();
        }

        public static void AnnulerCommande(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            commande.Statut = StatutCommande.Annulee;
            SauvegarderCommandes();
        }

        public static decimal CalculerPrixCommande(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            return commande.MontantTotal;
        }

        // Consultation des commandes
        public static List<Commande> ObtenirCommandes()
        {
            return _commandes;
        }

        public static Commande ObtenirCommande(string identifiant)
        {
            return TrouverCommande(identifiant);
        }

        private static Commande TrouverCommande(string identifiant)
        {
            var commande = _commandes.FirstOrDefault(c => c.Identifiant == identifiant);
            if (commande == null)
                throw new Exception("Commande non trouvée");
            return commande;
        }

        public static void AfficherCommande(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            Console.WriteLine($"\nDétails de la commande {commande.Identifiant}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Date : {commande.DateCommande}");
            Console.WriteLine($"Statut : {commande.Statut}");
            Console.WriteLine($"Station de livraison : {commande.Station.Nom}");
            Console.WriteLine("\nPlats commandés :");
            foreach (var ligne in commande.LignesCommande)
            {
                Console.WriteLine($"- {ligne.NomPlat} x{ligne.Quantite} : {ligne.SousTotal:C2}");
            }
            Console.WriteLine($"\nMontant total : {commande.MontantTotal:C2}");
            Console.WriteLine($"Chemin de livraison : {DeterminerCheminLivraison(commande.Identifiant)}");
        }

        public static string DeterminerCheminLivraison(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            var cuisinier = GestionCuisiniers.ObtenirCuisinier(commande.IdentifiantCuisinier);
            if (cuisinier == null)
                throw new Exception("Cuisinier non trouvé");

            var reseauMetro = ReseauMetro.Instance;
            var chemin = reseauMetro.ObtenirTrajet(cuisinier.Station.Id, commande.Station.Id);
            return $"Trajet de {cuisinier.Station.Nom} vers {commande.Station.Nom}";
        }
    }
} 