using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Client;

namespace LivinParis.Models.Cuisinier
{
    public class GestionCuisiniers
    {
        private static List<Cuisinier> _cuisiniers = new List<Cuisinier>();
        private static Dictionary<string, List<Models.Client.Client>> _clientsServis = new Dictionary<string, List<Models.Client.Client>>();
        private static Dictionary<string, List<string>> _platsRealises = new Dictionary<string, List<string>>();
        private static Dictionary<string, string> _platDuJour = new Dictionary<string, string>();

        public static Cuisinier ObtenirCuisinier(string identifiant)
        {
            return _cuisiniers.FirstOrDefault(c => c.Identifiant == identifiant);
        }

        // Gestion des cuisiniers
        public static void AjouterCuisinier(Cuisinier cuisinier)
        {
            if (cuisinier == null)
                throw new ArgumentNullException(nameof(cuisinier), "Le cuisinier ne peut pas être null");

            _cuisiniers.Add(cuisinier);
            _clientsServis[cuisinier.Identifiant] = new List<Models.Client.Client>();
            _platsRealises[cuisinier.Identifiant] = new List<string>();
        }

        public static void ModifierCuisinier(string identifiant, Cuisinier nouveauCuisinier)
        {
            if (nouveauCuisinier == null)
                throw new ArgumentNullException(nameof(nouveauCuisinier), "Le cuisinier ne peut pas être null");

            var cuisinierExistant = _cuisiniers.FirstOrDefault(c => c.Identifiant == identifiant);
            if (cuisinierExistant == null)
                throw new Exception("Cuisinier non trouvé");

            cuisinierExistant.Nom = nouveauCuisinier.Nom;
            cuisinierExistant.Prenom = nouveauCuisinier.Prenom;
            cuisinierExistant.Adresse = nouveauCuisinier.Adresse;
            cuisinierExistant.Telephone = nouveauCuisinier.Telephone;
            cuisinierExistant.Email = nouveauCuisinier.Email;
        }

        public static void SupprimerCuisinier(string identifiant)
        {
            var cuisinier = _cuisiniers.FirstOrDefault(c => c.Identifiant == identifiant);
            if (cuisinier == null)
                throw new Exception("Cuisinier non trouvé");

            _cuisiniers.Remove(cuisinier);
            _clientsServis.Remove(identifiant);
            _platsRealises.Remove(identifiant);
            _platDuJour.Remove(identifiant);
        }

        // Gestion des clients servis
        public static void AjouterClientServi(string identifiantCuisinier, Models.Client.Client client)
        {
            if (!_clientsServis.ContainsKey(identifiantCuisinier))
                throw new Exception("Cuisinier non trouvé");

            if (!_clientsServis[identifiantCuisinier].Contains(client))
            {
                _clientsServis[identifiantCuisinier].Add(client);
            }
        }

        public static List<Models.Client.Client> ObtenirClientsServis(string identifiantCuisinier)
        {
            if (!_clientsServis.ContainsKey(identifiantCuisinier))
                throw new Exception("Cuisinier non trouvé");

            return _clientsServis[identifiantCuisinier];
        }

        // Gestion des plats
        public static void AjouterPlatRealise(string identifiantCuisinier, string nomPlat)
        {
            if (!_platsRealises.ContainsKey(identifiantCuisinier))
                throw new Exception("Cuisinier non trouvé");

            _platsRealises[identifiantCuisinier].Add(nomPlat);
        }

        public static void DefinirPlatDuJour(string identifiantCuisinier, string plat)
        {
            if (!_cuisiniers.Any(c => c.Identifiant == identifiantCuisinier))
                throw new Exception("Cuisinier non trouvé");

            _platDuJour[identifiantCuisinier] = plat;
        }

        public static string ObtenirPlatDuJour(string identifiantCuisinier)
        {
            if (!_platDuJour.ContainsKey(identifiantCuisinier))
                return "Aucun plat du jour défini";

            return _platDuJour[identifiantCuisinier];
        }

        public static List<Cuisinier> ObtenirCuisiniers()
        {
            return _cuisiniers;
        }

        public static List<(string Plat, int Frequence)> ObtenirPlatsParFrequence(string identifiantCuisinier)
        {
            if (!_platsRealises.ContainsKey(identifiantCuisinier))
                throw new Exception("Cuisinier non trouvé");

            return _platsRealises[identifiantCuisinier]
                .GroupBy(p => p)
                .Select(g => (Plat: g.Key, Frequence: g.Count()))
                .OrderByDescending(x => x.Frequence)
                .ToList();
        }

        // Affichage
        public static void AfficherStatistiquesCuisinier(string identifiantCuisinier)
        {
            var cuisinier = _cuisiniers.FirstOrDefault(c => c.Identifiant == identifiantCuisinier);
            if (cuisinier == null)
                throw new Exception("Cuisinier non trouvé");

            Console.WriteLine($"\nStatistiques pour {cuisinier.Prenom} {cuisinier.Nom}");
            Console.WriteLine("----------------------------------------");

            // Afficher le plat du jour
            Console.WriteLine($"Plat du jour : {ObtenirPlatDuJour(identifiantCuisinier)}");

            // Afficher les clients servis
            Console.WriteLine("\nClients servis :");
            var clients = ObtenirClientsServis(identifiantCuisinier);
            foreach (var client in clients)
            {
                Console.WriteLine($"- {client.Prenom} {client.Nom}");
            }

            // Afficher les plats par fréquence
            Console.WriteLine("\nPlats réalisés (par fréquence) :");
            var plats = ObtenirPlatsParFrequence(identifiantCuisinier);
            foreach (var plat in plats)
            {
                Console.WriteLine($"- {plat.Plat} : {plat.Frequence} fois");
            }
        }
    }
} 