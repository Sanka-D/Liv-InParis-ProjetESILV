using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Models.Commande
{
    public class GestionCommandes
    {
        private static List<Commande> _commandes = new List<Commande>();

        public static void ReinitialiserCommandes()
        {
            _commandes.Clear();
        }

        public static List<Commande> ObtenirCommandes()
        {
            return _commandes;
        }

        // Création et modification de commandes
        public static Commande CreerCommande(string identifiantClient, string adresseLivraison)
        {
            // Vérifier l'existence du client
            if (!VerifierClient(identifiantClient))
                throw new Exception("Client non trouvé");

            var commande = new Commande(identifiantClient, adresseLivraison);
            _commandes.Add(commande);
            return commande;
        }

        public static void AssignerCuisinier(string identifiantCommande, string identifiantCuisinier)
        {
            var commande = _commandes.FirstOrDefault(c => c.Identifiant == identifiantCommande);
            if (commande == null)
                throw new Exception("Commande non trouvée");

            commande.IdentifiantCuisinier = identifiantCuisinier;
            commande.Statut = StatutCommande.EnPreparation;
        }

        // Simulation des étapes
        public static void DemarrerPreparation(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            if (commande.IdentifiantCuisinier == null)
                throw new Exception("Aucun cuisinier assigné à la commande");

            commande.Statut = StatutCommande.EnPreparation;
        }

        public static void DemarrerLivraison(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            if (commande.Statut != StatutCommande.EnPreparation)
                throw new Exception("La commande n'est pas prête pour la livraison");

            commande.Statut = StatutCommande.EnLivraison;
        }

        public static void TerminerCommande(string identifiantCommande)
        {
            var commande = _commandes.FirstOrDefault(c => c.Identifiant == identifiantCommande);
            if (commande == null)
                throw new Exception("Commande non trouvée");

            commande.Statut = StatutCommande.Livree;

            // Mettre à jour le montant des achats du client
            if (VerifierClient(commande.IdentifiantClient))
            {
                var client = GestionClients.ObtenirClientsParOrdreAlphabetique()
                    .FirstOrDefault(c => c.Identifiant == commande.IdentifiantClient);
                if (client != null)
                {
                    client.AjouterAchat(commande.MontantTotal);
                }
            }
        }

        // Calcul de prix et optimisation
        public static decimal CalculerPrixTotal(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            return commande.MontantTotal;
        }

        public static string DeterminerCheminLivraison(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            // Simulation simple du chemin de livraison
            return $"Restaurant → {commande.AdresseLivraison}";
        }

        // Méthodes utilitaires
        private static Commande TrouverCommande(string identifiantCommande)
        {
            var commande = _commandes.FirstOrDefault(c => c.Identifiant == identifiantCommande);
            if (commande == null)
                throw new Exception("Commande non trouvée");
            return commande;
        }

        private static bool VerifierClient(string identifiantClient)
        {
            return GestionClients.ObtenirClientsParOrdreAlphabetique()
                .Any(c => c.Identifiant == identifiantClient);
        }

        private static bool VerifierCuisinier(string identifiantCuisinier)
        {
            // Cette vérification nécessite une méthode dans GestionCuisiniers
            // Pour l'instant, on retourne true
            return true;
        }

        // Méthodes d'affichage
        public static void AfficherCommande(string identifiantCommande)
        {
            var commande = TrouverCommande(identifiantCommande);
            Console.WriteLine($"\nDétails de la commande {commande.Identifiant}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Date : {commande.DateCommande}");
            Console.WriteLine($"Statut : {commande.Statut}");
            Console.WriteLine($"Adresse de livraison : {commande.AdresseLivraison}");
            Console.WriteLine("\nPlats commandés :");
            foreach (var ligne in commande.LignesCommande)
            {
                Console.WriteLine($"- {ligne.NomPlat} x{ligne.Quantite} : {ligne.SousTotal:C2}");
            }
            Console.WriteLine($"\nMontant total : {commande.MontantTotal:C2}");
            Console.WriteLine($"Chemin de livraison : {DeterminerCheminLivraison(commande.Identifiant)}");
        }
    }
} 