using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Commande;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Models.Statistiques
{
    public static class GestionStatistiques
    {
        public class StatistiquesGlobales
        {
            public int NombreTotalCommandes { get; set; }
            public decimal MontantTotalCommandes { get; set; }
        }

        public class StatistiquesClient
        {
            public int NombreCommandes { get; set; }
            public decimal MontantTotal { get; set; }
        }

        public class StatistiquesCuisinier
        {
            public int NombreCommandes { get; set; }
            public decimal MontantTotal { get; set; }
        }

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
            if (commandes.Count == 0) return 0;
            return commandes.Average(c => c.MontantTotal);
        }

        // Statistiques des moyennes des comptes clients
        public static decimal CalculerMoyenneComptesClients()
        {
            var commandes = GestionCommandes.ObtenirCommandes();
            if (commandes.Count == 0) return 0;

            var montantsParClient = commandes
                .GroupBy(c => c.IdentifiantClient)
                .Select(g => g.Sum(c => c.MontantTotal))
                .ToList();

            return montantsParClient.Average();
        }

        // Statistiques des commandes par nationalité des plats
        public static Dictionary<string, int> ObtenirCommandesParNationalitePlats()
        {
            var commandes = GestionCommandes.ObtenirCommandes();
            var platsParNationalite = new Dictionary<string, int>();

            foreach (var commande in commandes)
            {
                foreach (var ligne in commande.LignesCommande)
                {
                    var nationalite = DeterminerNationalitePlat(ligne.NomPlat);
                    if (!platsParNationalite.ContainsKey(nationalite))
                        platsParNationalite[nationalite] = 0;
                    platsParNationalite[nationalite]++;
                }
            }

            return platsParNationalite;
        }

        // Méthode utilitaire pour déterminer la nationalité d'un plat
        private static string DeterminerNationalitePlat(string nomPlat)
        {
            var platsFrancais = new[] { "Coq au vin", "Ratatouille", "Quiche", "Soupe à l'oignon" };
            var platsItaliens = new[] { "Pizza", "Pasta", "Risotto", "Lasagne" };
            var platsJaponais = new[] { "Sushi", "Ramen", "Tempura", "Maki" };

            if (platsFrancais.Any(p => nomPlat.Contains(p, StringComparison.OrdinalIgnoreCase)))
                return "Française";
            if (platsItaliens.Any(p => nomPlat.Contains(p, StringComparison.OrdinalIgnoreCase)))
                return "Italienne";
            if (platsJaponais.Any(p => nomPlat.Contains(p, StringComparison.OrdinalIgnoreCase)))
                return "Japonaise";

            return "Autre";
        }

        public static StatistiquesGlobales ObtenirStatistiquesGlobales()
        {
            var commandes = GestionCommandes.ObtenirCommandes();
            return new StatistiquesGlobales
            {
                NombreTotalCommandes = commandes.Count,
                MontantTotalCommandes = commandes.Sum(c => c.MontantTotal)
            };
        }

        public static StatistiquesClient ObtenirStatistiquesParClient(string identifiantClient)
        {
            var commandes = GestionCommandes.ObtenirCommandes()
                .Where(c => c.IdentifiantClient == identifiantClient)
                .ToList();

            return new StatistiquesClient
            {
                NombreCommandes = commandes.Count,
                MontantTotal = commandes.Sum(c => c.MontantTotal)
            };
        }

        public static StatistiquesCuisinier ObtenirStatistiquesParCuisinier(string identifiantCuisinier)
        {
            var commandes = GestionCommandes.ObtenirCommandes()
                .Where(c => c.IdentifiantCuisinier == identifiantCuisinier)
                .ToList();

            return new StatistiquesCuisinier
            {
                NombreCommandes = commandes.Count,
                MontantTotal = commandes.Sum(c => c.MontantTotal)
            };
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

        public static void AfficherStatistiques()
        {
            var commandes = GestionCommandes.ObtenirCommandes();
            Console.WriteLine("\n=== STATISTIQUES GLOBALES ===");
            Console.WriteLine($"Nombre total de commandes : {commandes.Count}");
            Console.WriteLine($"Montant total des commandes : {commandes.Sum(c => c.MontantTotal):C2}");
        }

        public static void AfficherStatistiquesParClient(string identifiantClient)
        {
            var commandes = GestionCommandes.ObtenirCommandes()
                .Where(c => c.IdentifiantClient == identifiantClient)
                .ToList();

            Console.WriteLine("\n=== STATISTIQUES PAR CLIENT ===");
            Console.WriteLine($"Nombre de commandes : {commandes.Count}");
            Console.WriteLine($"Montant total : {commandes.Sum(c => c.MontantTotal):C2}");
        }

        public static void AfficherStatistiquesParCuisinier(string identifiantCuisinier)
        {
            var commandes = GestionCommandes.ObtenirCommandes()
                .Where(c => c.IdentifiantCuisinier == identifiantCuisinier)
                .ToList();

            Console.WriteLine("\n=== STATISTIQUES PAR CUISINIER ===");
            Console.WriteLine($"Nombre de commandes : {commandes.Count}");
            Console.WriteLine($"Montant total : {commandes.Sum(c => c.MontantTotal):C2}");
        }
    }
} 