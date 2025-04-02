using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Trajets;
using LivinParis.Models.Statistiques;
using System.Text.Json;
using System.IO;

namespace LivinParis.Models.Client
{
    public class GestionClients
    {
        private static List<Client> _clients = new List<Client>();
        private static ReseauMetro _reseauMetro;
        private static readonly string _cheminFichier = "./Data/clients.json";

        public static void Initialiser(ReseauMetro reseauMetro)
        {
            _reseauMetro = reseauMetro;
            ChargerClients();
        }

        // Réinitialiser la liste des clients
        public static void ReinitialiserClients()
        {
            _clients.Clear();
            SauvegarderClients();
        }

        // Ajouter un client
        public static void AjouterClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Le client ne peut pas être null");

            if (client.Station == null)
                throw new ArgumentException("La station ne peut pas être null");

            _clients.Add(client);
            SauvegarderClients();
        }

        // Modifier un client
        public static void ModifierClient(string identifiant, Client nouveauClient)
        {
            if (nouveauClient == null)
                throw new ArgumentNullException(nameof(nouveauClient), "Le client ne peut pas être null");

            if (nouveauClient.Station == null)
                throw new ArgumentException("La station ne peut pas être null");

            var clientExistant = _clients.FirstOrDefault(c => c.Identifiant == identifiant);
            if (clientExistant == null)
                throw new Exception("Client non trouvé");

            clientExistant.Nom = nouveauClient.Nom;
            clientExistant.Prenom = nouveauClient.Prenom;
            clientExistant.Station = nouveauClient.Station;
            clientExistant.Telephone = nouveauClient.Telephone;
            clientExistant.Email = nouveauClient.Email;
            clientExistant.EstEntreprise = nouveauClient.EstEntreprise;
            clientExistant.NomEntreprise = nouveauClient.NomEntreprise;
            clientExistant.ReferentEntreprise = nouveauClient.ReferentEntreprise;

            SauvegarderClients();
        }

        // Supprimer un client
        public static void SupprimerClient(string identifiant)
        {
            var client = _clients.FirstOrDefault(c => c.Identifiant == identifiant);
            if (client == null)
                throw new Exception("Client non trouvé");

            _clients.Remove(client);
            SauvegarderClients();
        }

        // Obtenir tous les clients triés par nom et prénom
        public static List<Client> ObtenirClientsParOrdreAlphabetique()
        {
            return _clients
                .OrderBy(c => c.Nom)
                .ThenBy(c => c.Prenom)
                .ToList();
        }

        // Obtenir tous les clients triés par station
        public static List<Client> ObtenirClientsParStation()
        {
            return _clients
                .OrderBy(c => c.Station.Nom)
                .ToList();
        }

        // Obtenir tous les clients triés par montant des achats
        public static List<Client> ObtenirClientsParMontantAchats()
        {
            return _clients
                .OrderByDescending(c => c.MontantAchats)
                .ToList();
        }

        // Obtenir tous les clients triés par rue
        public static List<Client> ObtenirClientsParRue()
        {
            return _clients
                .OrderBy(c => c.Station.Adresse)
                .ToList();
        }

        // Afficher les clients selon le mode de tri choisi
        public static void AfficherClients(string tri = "alphabetique")
        {
            var clients = tri switch
            {
                "alphabetique" => ObtenirClientsParOrdreAlphabetique(),
                "rue" => ObtenirClientsParRue(),
                "montant" => ObtenirClientsParMontantAchats(),
                _ => throw new ArgumentException("Type de tri non valide")
            };

            Console.WriteLine("\n=== LISTE DES CLIENTS ===");
            if (clients.Count == 0)
            {
                Console.WriteLine("Aucun client enregistré.");
                return;
            }

            foreach (var client in clients)
            {
                Console.WriteLine($"\nClient : {client.Prenom} {client.Nom}");
                Console.WriteLine($"ID : {client.Identifiant}");
                Console.WriteLine($"Station : {client.Station.Nom} ({client.Station.Adresse})");
                Console.WriteLine($"Téléphone : {client.Telephone}");
                Console.WriteLine($"Email : {client.Email}");
                
                // Afficher les statistiques du client
                var stats = GestionStatistiques.ObtenirStatistiquesParClient(client.Identifiant);
                Console.WriteLine($"Nombre de commandes : {stats.NombreCommandes}");
                Console.WriteLine($"Montant total des commandes : {stats.MontantTotal:C2}");
            }
        }

        public static Client ObtenirClient(string identifiant)
        {
            var client = _clients.FirstOrDefault(c => c.Identifiant == identifiant);
            if (client == null)
                throw new Exception("Client non trouvé");
            return client;
        }

        // Obtenir tous les clients
        public static List<Client> ObtenirClients()
        {
            return _clients;
        }

        private static void SauvegarderClients()
        {
            try
            {
                // S'assurer que le dossier Data existe
                var dossierData = Path.GetDirectoryName(_cheminFichier);
                if (!Directory.Exists(dossierData))
                {
                    Directory.CreateDirectory(dossierData);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(_clients, options);
                File.WriteAllText(_cheminFichier, jsonString);
                Console.WriteLine("Clients sauvegardés avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde des clients : {ex.Message}");
            }
        }

        private static void ChargerClients()
        {
            try
            {
                if (File.Exists(_cheminFichier))
                {
                    var jsonString = File.ReadAllText(_cheminFichier);
                    _clients = JsonSerializer.Deserialize<List<Client>>(jsonString);
                    Console.WriteLine("Clients chargés avec succès.");
                }
                else
                {
                    Console.WriteLine("Aucun fichier de clients trouvé. Une nouvelle liste sera créée.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des clients : {ex.Message}");
            }
        }
    }
} 