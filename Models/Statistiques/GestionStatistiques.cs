using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Commande;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Models.Statistiques
{
    public class GestionStatistiques
    {
        // Statistiques des livraisons par cuisinier
        public static Dictionary<string, int> ObtenirLivraisonsParCuisinier()
        {
            var commandes = GestionCommandes.ObtenirCommandes()
                .Where(c => c.Statut == StatutCommande.Livree)
                .GroupBy(c => c.IdentifiantCuisinier)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );
            return commandes;
        }

        // Statistiques des commandes sur une période
        public static List<Commande.Commande> ObtenirCommandesSurPeriode(DateTime debut, DateTime fin)
        {
            return GestionCommandes.ObtenirCommandes()
                .Where(c => c.DateCommande >= debut && c.DateCommande <= fin)
                .OrderBy(c => c.DateCommande)
                .ToList();
        }

        // Statistiques des moyennes des prix
        public static decimal CalculerMoyennePrixCommandes()
        {
            var commandes = GestionCommandes.ObtenirCommandes();
            if (!commandes.Any()) return 0;

            decimal sommeCommandes = commandes.Sum(c => c.MontantTotal);
            return sommeCommandes / commandes.Count;
        }

        // Statistiques des moyennes des comptes clients
        public static decimal CalculerMoyenneComptesClients()
        {
            var clients = GestionClients.ObtenirClientsParOrdreAlphabetique();
            if (!clients.Any()) return 0;
            return clients.Average(c => c.MontantAchats);
        }

        // Statistiques des commandes par nationalité des plats
        public static Dictionary<string, int> ObtenirCommandesParNationalitePlats()
        {
            var commandesParNationalite = new Dictionary<string, int>();
            
            foreach (var commande in GestionCommandes.ObtenirCommandes())
            {
                foreach (var ligne in commande.LignesCommande)
                {
                    var nationalite = DeterminerNationalitePlat(ligne.NomPlat);
                    if (!commandesParNationalite.ContainsKey(nationalite))
                    {
                        commandesParNationalite[nationalite] = 0;
                    }
                    commandesParNationalite[nationalite] += ligne.Quantite;
                }
            }

            return commandesParNationalite;
        }

        // Méthode utilitaire pour déterminer la nationalité d'un plat
        private static string DeterminerNationalitePlat(string nomPlat)
        {
            // Liste non exhaustive, à compléter selon les besoins
            if (nomPlat.ToLower().Contains("coq au vin") || 
                nomPlat.ToLower().Contains("ratatouille") ||
                nomPlat.ToLower().Contains("bouillabaisse"))
                return "Française";
            
            if (nomPlat.ToLower().Contains("pizza") || 
                nomPlat.ToLower().Contains("pasta") ||
                nomPlat.ToLower().Contains("risotto"))
                return "Italienne";
            
            if (nomPlat.ToLower().Contains("sushi") || 
                nomPlat.ToLower().Contains("ramen") ||
                nomPlat.ToLower().Contains("tempura"))
                return "Japonaise";

            return "Autre";
        }

        // Méthode d'affichage des statistiques
        public static void AfficherStatistiques(DateTime? debut = null, DateTime? fin = null)
        {
            debut ??= DateTime.MinValue;
            fin ??= DateTime.MaxValue;

            Console.WriteLine("\n=== STATISTIQUES LIVINPARIS ===");
            Console.WriteLine("===============================");

            // Livraisons par cuisinier
            Console.WriteLine("\nLivraisons par cuisinier :");
            foreach (var kvp in ObtenirLivraisonsParCuisinier())
            {
                var cuisinier = GestionCuisiniers.ObtenirCuisinier(kvp.Key);
                string nomCuisinier = cuisinier != null ? $"{cuisinier.Prenom} {cuisinier.Nom}" : "Cuisinier inconnu";
                Console.WriteLine($"- {nomCuisinier} : {kvp.Value} livraison(s)");
            }

            // Commandes sur la période
            var commandesPeriode = ObtenirCommandesSurPeriode(debut.Value, fin.Value);
            Console.WriteLine($"\nCommandes sur la période ({debut.Value:d} - {fin.Value:d}) :");
            Console.WriteLine($"- Nombre total de commandes : {commandesPeriode.Count}");
            Console.WriteLine($"- Montant total : {commandesPeriode.Sum(c => c.MontantTotal):C2}");

            // Moyennes
            Console.WriteLine("\nMoyennes :");
            Console.WriteLine($"- Prix moyen des commandes : {CalculerMoyennePrixCommandes():C2}");
            Console.WriteLine($"- Moyenne des comptes clients : {CalculerMoyenneComptesClients():C2}");

            // Commandes par nationalité
            Console.WriteLine("\nCommandes par nationalité des plats :");
            foreach (var kvp in ObtenirCommandesParNationalitePlats().OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"- Cuisine {kvp.Key} : {kvp.Value} plat(s)");
            }
        }
    }
} 