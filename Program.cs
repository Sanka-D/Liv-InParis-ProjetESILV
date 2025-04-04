﻿using LivinParis.Data;
using LivinParis.Models;
using System;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;
using LivinParis.Models.Trajets;
using System.IO;

namespace LivinParis
{
    class Program
    {
        private static DatabaseManager _dbManager;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Tentative de connexion à la base de données...");
                Console.WriteLine("Serveur: 127.0.0.1:3307");
                Console.WriteLine("Base de données: livinparis");
                Console.WriteLine("Utilisateur: root");

                /// Configuration de la connexion à la base de données
                _dbManager = new DatabaseManager("127.0.0.1", "livinparis", "root", "270115");
                _dbManager.Connect();

                /// TODO: Implémenter la vérification des identifiants
                Console.WriteLine("Connexion réussie!");

                while (true)
                {
                    Console.WriteLine("\n=== Système de Gestion Liv-In Paris ===");
                    Console.WriteLine("1. Se connecter");
                    Console.WriteLine("2. Créer un compte");
                    Console.WriteLine("3. Visualiser la base de données");
                    Console.WriteLine("4. Gestion des trajets");
                    Console.WriteLine("5. Quitter");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Login();
                            break;
                        case "2":
                            CreateAccount();
                            break;
                        case "3":
                            ViewDatabase();
                            break;
                        case "4":
                            GestionTrajets();
                            break;
                        case "5":
                            _dbManager.Disconnect();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Option invalide");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur détaillée: {ex.Message}");
                Console.WriteLine("\nVeuillez vérifier que:");
                Console.WriteLine("1. MySQL est en cours d'exécution sur le port 3307");
                Console.WriteLine("2. La base de données 'livinparis' existe");
                Console.WriteLine("3. Les identifiants sont corrects (root/root)");
                Console.WriteLine("\nAppuyez sur une touche pour quitter...");
                Console.ReadKey();
            }
        }

        static void ViewDatabase()
        {
            Console.WriteLine("\n=== Visualisation de la base de données ===");
            Console.WriteLine("1. Afficher les cuisiniers");
            Console.WriteLine("2. Afficher les clients");
            Console.WriteLine("3. Afficher les plats");
            Console.WriteLine("4. Retour au menu principal");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayCuisiniers();
                    break;
                case "2":
                    DisplayClients();
                    break;
                case "3":
                    DisplayPlats();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Option invalide");
                    break;
            }
        }

        static void DisplayCuisiniers()
        {
            Console.WriteLine("\n=== Liste des cuisiniers ===");
            using (var reader = _dbManager.ExecuteReader("SELECT * FROM cuisiniers"))
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Nom: {reader["nom"]} {reader["prenom"]}");
                    Console.WriteLine($"Spécialité: {reader["specialite"]}");
                    Console.WriteLine($"Expérience: {reader["experience"]} ans");
                    Console.WriteLine($"Email: {reader["email"]}");
                    Console.WriteLine($"Téléphone: {reader["telephone"]}");
                    Console.WriteLine("-------------------");
                }
            }
        }

        static void DisplayClients()
        {
            Console.WriteLine("\n=== Liste des clients ===");
            using (var reader = _dbManager.ExecuteReader("SELECT * FROM clients"))
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Nom: {reader["nom"]} {reader["prenom"]}");
                    Console.WriteLine($"Adresse: {reader["adresse"]}");
                    Console.WriteLine($"Email: {reader["email"]}");
                    Console.WriteLine($"Téléphone: {reader["telephone"]}");
                    Console.WriteLine($"Préférences: {reader["preferences"]}");
                    Console.WriteLine("-------------------");
                }
            }
        }

        static void DisplayPlats()
        {
            Console.WriteLine("\n=== Liste des plats ===");
            using (var reader = _dbManager.ExecuteReader("SELECT p.*, c.nom as cuisinier_nom, c.prenom as cuisinier_prenom FROM plats p JOIN cuisiniers c ON p.cuisinier_id = c.id"))
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Nom: {reader["nom"]}");
                    Console.WriteLine($"Description: {reader["description"]}");
                    Console.WriteLine($"Prix: {reader["prix"]}€");
                    Console.WriteLine($"Type: {reader["type"]}");
                    Console.WriteLine($"Cuisinier: {reader["cuisinier_nom"]} {reader["cuisinier_prenom"]}");
                    Console.WriteLine("-------------------");
                }
            }
        }

        static void Login()
        {
            Console.WriteLine("\n=== Connexion ===");
            Console.Write("Nom d'utilisateur: ");
            var username = Console.ReadLine();
            Console.Write("Mot de passe: ");
            var password = Console.ReadLine();

            /// TODO: Implémenter la vérification des identifiants
            Console.WriteLine("Connexion réussie!");
            
            while (true)
            {
                Console.WriteLine("\n=== Menu Utilisateur ===");
                Console.WriteLine("1. Visualiser les plats");
                Console.WriteLine("2. Créer une commande");
                Console.WriteLine("3. Calculer un trajet vers le restaurant");
                Console.WriteLine("4. Déconnexion");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayPlats();
                        break;
                    case "2":
                        CreateOrder();
                        break;
                    case "3":
                        CalculerTrajetVersRestaurant();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Option invalide");
                        break;
                }
            }
        }

        static void CalculerTrajetVersRestaurant()
        {
            try
            {
                Console.WriteLine("\n=== Calculer un trajet vers le restaurant ===");
                var reseauMetro = LivinParis.Models.Trajets.ChargementXML.ChargerReseau("/home/sanka/Liv-InParis-ProjetESILV/Data/metro.xml");
                
                Console.WriteLine("Stations disponibles:");
                foreach (var station in reseauMetro.Stations.Values)
                {
                    Console.WriteLine($"- {station.Nom}");
                }

                Console.Write("\nVotre station de départ: ");
                var depart = Console.ReadLine();
                Console.WriteLine("Le restaurant se trouve à Châtelet.");
                
                var stationDepart = reseauMetro.RechercherStationParNom(depart);
                var stationArrivee = reseauMetro.RechercherStationParNom("Châtelet");

                if (stationDepart == null)
                {
                    Console.WriteLine("Station de départ non trouvée.");
                    return;
                }

                var chemin = reseauMetro.Graphe.Dijkstra(stationDepart, stationArrivee);
                
                if (chemin == null || chemin.Count == 0)
                {
                    Console.WriteLine("Aucun chemin trouvé vers le restaurant.");
                    return;
                }

                Console.WriteLine("\nChemin vers le restaurant:");
                foreach (var station in chemin)
                {
                    Console.WriteLine($"- {station.Nom}");
                }
                Console.WriteLine($"Distance totale: {reseauMetro.Graphe.CalculerDistanceTotale(chemin):F2} km");
                Console.WriteLine($"Temps estimé: {(reseauMetro.Graphe.CalculerDistanceTotale(chemin) / 0.5):F0} minutes");
                
                // Afficher le graphe du chemin
                AfficherGrapheChemin(chemin);
                
                /// Générer une visualisation graphique du trajet
                Console.WriteLine("\nGénération d'une visualisation graphique du trajet vers le restaurant...");
                var visualisation = new VisualisationReseau(reseauMetro);
                visualisation.DessinerTrajet(chemin);
                
                /// Créer un dossier pour les visualisations si nécessaire
                string dossierVisualisation = "Visualisations";
                if (!Directory.Exists(dossierVisualisation))
                {
                    Directory.CreateDirectory(dossierVisualisation);
                }
                
                /// Nom du fichier avec date et heure
                string nomFichier = $"{dossierVisualisation}/trajet_vers_restaurant_{stationDepart.Nom}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                visualisation.SauvegarderImage(nomFichier);
                Console.WriteLine($"Visualisation du trajet sauvegardée dans : {nomFichier}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du calcul du trajet: {ex.Message}");
            }
        }

        static void CreateOrder()
        {
            Console.WriteLine("\n=== Création d'une commande ===");
            DisplayPlats();
            
            Console.Write("\nEntrez l'ID du plat que vous souhaitez commander: ");
            if (int.TryParse(Console.ReadLine(), out int platId))
            {
                Console.Write("Quantité: ");
                if (int.TryParse(Console.ReadLine(), out int quantite))
                {
                    /// TODO: Implémenter la création de la commande dans la base de données
                    Console.WriteLine("Commande créée avec succès!");
                }
                else
                {
                    Console.WriteLine("Quantité invalide");
                }
            }
            else
            {
                Console.WriteLine("ID de plat invalide");
            }
        }

        static void CreateAccount()
        {
            Console.WriteLine("\n=== Création de compte ===");
            Console.Write("Nom d'utilisateur: ");
            var username = Console.ReadLine();
            Console.Write("Mot de passe: ");
            var password = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Rôle (1: Admin, 2: Manager, 3: User): ");
            var roleChoice = Console.ReadLine();

            UserRole role = roleChoice switch
            {
                "1" => UserRole.Admin,
                "2" => UserRole.Manager,
                "3" => UserRole.User,
                _ => UserRole.User
            };

            /// TODO: Implémenter la création du compte
            Console.WriteLine("Compte créé avec succès!");
        }

        static void GestionTrajets()
        {
            try
            {
                Console.WriteLine("\nChargement du réseau de métro...");
                var reseauMetro = LivinParis.Models.Trajets.ChargementXML.ChargerReseau("/home/sanka/Liv-InParis-ProjetESILV/Data/metro.xml");
                Console.WriteLine("Réseau chargé avec succès!");

                while (true)
                {
                    Console.WriteLine("\n=== Gestion des Trajets ===");
                    Console.WriteLine("1. Rechercher le plus court chemin");
                    Console.WriteLine("2. Visualiser le réseau");
                    Console.WriteLine("3. Comparer les algorithmes");
                    Console.WriteLine("4. Retour au menu principal");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            RechercherChemin(reseauMetro);
                            break;
                        case "2":
                            VisualiserReseau(reseauMetro);
                            break;
                        case "3":
                            ComparerAlgorithmes(reseauMetro);
                            break;
                        case "4":
                            return;
                        default:
                            Console.WriteLine("Option invalide");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du réseau: {ex.Message}");
            }
        }

        static void RechercherChemin(LivinParis.Models.Trajets.ReseauMetro reseau)
        {
            Console.WriteLine("\n=== Recherche de chemin ===");
            Console.WriteLine("Stations disponibles:");
            foreach (var station in reseau.Stations.Values)
            {
                Console.WriteLine($"- {station.Nom}");
            }

            Console.Write("\nStation de départ: ");
            var depart = Console.ReadLine();
            Console.Write("Station d'arrivée: ");
            var arrivee = Console.ReadLine();

            var stationDepart = reseau.RechercherStationParNom(depart);
            var stationArrivee = reseau.RechercherStationParNom(arrivee);

            if (stationDepart == null || stationArrivee == null)
            {
                Console.WriteLine("Une ou plusieurs stations n'ont pas été trouvées.");
                return;
            }

            Console.WriteLine("\nChoisissez l'algorithme:");
            Console.WriteLine("1. Dijkstra");
            Console.WriteLine("2. Bellman-Ford");
            Console.WriteLine("3. Floyd-Warshall");

            var algoChoice = Console.ReadLine();
            List<LivinParis.Models.Trajets.Station> chemin = null;

            switch (algoChoice)
            {
                case "1":
                    chemin = reseau.Graphe.Dijkstra(stationDepart, stationArrivee);
                    break;
                case "2":
                    chemin = reseau.Graphe.BellmanFord(stationDepart, stationArrivee);
                    break;
                case "3":
                    chemin = reseau.Graphe.FloydWarshall(stationDepart, stationArrivee);
                    break;
                default:
                    Console.WriteLine("Algorithme invalide");
                    return;
            }

            if (chemin == null || chemin.Count == 0)
            {
                Console.WriteLine("Aucun chemin trouvé entre ces stations.");
                return;
            }

            Console.WriteLine("\nChemin trouvé:");
            foreach (var station in chemin)
            {
                Console.WriteLine($"- {station.Nom}");
            }
            Console.WriteLine($"Distance totale: {reseau.Graphe.CalculerDistanceTotale(chemin):F2} km");
            
            // Afficher le graphe du chemin
            AfficherGrapheChemin(chemin);
            
            /// Générer une visualisation graphique du trajet
            Console.WriteLine("\nGénération d'une visualisation graphique du trajet...");
            var visualisation = new VisualisationReseau(reseau);
            visualisation.DessinerTrajet(chemin);
            
            /// Créer un dossier pour les visualisations si nécessaire
            string dossierVisualisation = "Visualisations";
            if (!Directory.Exists(dossierVisualisation))
            {
                Directory.CreateDirectory(dossierVisualisation);
            }
            
            string algorithme = algoChoice switch
            {
                "1" => "Dijkstra",
                "2" => "BellmanFord",
                "3" => "FloydWarshall",
                _ => "Inconnu"
            };
            
            /// Nom du fichier avec date, heure et algorithme utilisé
            string nomFichier = $"{dossierVisualisation}/trajet_{stationDepart.Nom}_vers_{stationArrivee.Nom}_{algorithme}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            visualisation.SauvegarderImage(nomFichier);
            Console.WriteLine($"Visualisation du trajet sauvegardée dans : {nomFichier}");
        }

        static void AfficherGrapheChemin(List<LivinParis.Models.Trajets.Station> chemin)
        {
            if (chemin == null || chemin.Count < 2)
                return;

            Console.WriteLine("\n=== Visualisation du chemin ===");
            Console.WriteLine();

            for (int i = 0; i < chemin.Count - 1; i++)
            {
                var stationActuelle = chemin[i];
                var stationSuivante = chemin[i + 1];
                var distance = stationActuelle.CalculerDistance(stationSuivante);

                /// Station de départ
                Console.WriteLine($"[{stationActuelle.Nom}]");
                
                /// Ligne de connexion avec la distance
                Console.WriteLine($"    |");
                Console.WriteLine($"    | {distance:F2} km");
                Console.WriteLine($"    |");
                Console.WriteLine($"    V");

                /// Station d'arrivée (seulement la dernière fois)
                if (i == chemin.Count - 2)
                {
                    Console.WriteLine($"[{stationSuivante.Nom}]");
                }
            }
            
            Console.WriteLine("\n=== Fin de la visualisation ===");
        }

        static void VisualiserReseau(LivinParis.Models.Trajets.ReseauMetro reseau)
        {
            Console.WriteLine("\n=== Visualisation du réseau ===");
            
            /// Obtenir les données du réseau
            var stations = reseau.Stations.Values.ToList();
            var connexions = reseau.ObtenirLignes();
            
            Console.WriteLine($"Nombre de stations: {stations.Count}");
            Console.WriteLine($"Nombre de connexions: {connexions.Count}");
            Console.WriteLine();
            
            /// Créer une représentation ASCII du réseau
            AfficherGrapheReseau(stations, connexions);
            
            /// Afficher la liste des stations pour référence
            Console.WriteLine("\nListe des stations:");
            foreach (var station in stations)
            {
                Console.WriteLine($"- {station.Id}: {station.Nom} ({station.Adresse})");
            }
            
            /// Générer une visualisation graphique du réseau complet
            Console.WriteLine("\nGénération d'une visualisation graphique du réseau complet...");
            var visualisation = new VisualisationReseau(reseau);
            
            /// Créer un dossier pour les visualisations si nécessaire
            string dossierVisualisation = "Visualisations";
            if (!Directory.Exists(dossierVisualisation))
            {
                Directory.CreateDirectory(dossierVisualisation);
            }
            
            /// Nom du fichier avec date et heure
            string nomFichier = $"{dossierVisualisation}/reseau_complet_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            visualisation.SauvegarderImage(nomFichier);
            Console.WriteLine($"Visualisation du réseau sauvegardée dans : {nomFichier}");
        }

        static void AfficherGrapheReseau(List<LivinParis.Models.Trajets.Station> stations, List<LivinParis.Models.Trajets.Ligne> connexions)
        {
            Console.WriteLine("=== Graphe du réseau ===");
            Console.WriteLine("Légende: [id:nom] --- distance en km ---> [id:nom]");
            Console.WriteLine();

            /// Trier les connexions par station de départ pour une meilleure lisibilité
            var connexionsTriees = connexions.OrderBy(c => c.StationDepart.Id).ToList();
            
            /// Afficher chaque connexion
            foreach (var connexion in connexionsTriees)
            {
                Console.WriteLine($"[{connexion.StationDepart.Id}:{connexion.StationDepart.Nom}] --- {connexion.Duree:F2} km ---> [{connexion.StationArrivee.Id}:{connexion.StationArrivee.Nom}]");
            }
            
            Console.WriteLine();
            
            /// Afficher une matrice d'adjacence simplifiée pour les 10 premières stations (si plus de 10)
            if (stations.Count > 0)
            {
                var stationsMatrice = stations.Take(Math.Min(10, stations.Count)).ToList();
                Console.WriteLine("Matrice d'adjacence (10 premières stations max):");
                
                /// Entête avec IDs des stations
                Console.Write("    ");
                foreach (var s in stationsMatrice)
                {
                    Console.Write($"{s.Id.ToString().PadLeft(4)} ");
                }
                Console.WriteLine();
                
                /// Lignes de la matrice
                foreach (var s1 in stationsMatrice)
                {
                    Console.Write($"{s1.Id.ToString().PadLeft(4)} ");
                    
                    foreach (var s2 in stationsMatrice)
                    {
                        bool connexionExiste = connexions.Any(c => 
                            (c.StationDepart.Id == s1.Id && c.StationArrivee.Id == s2.Id) || 
                            (c.StationDepart.Id == s2.Id && c.StationArrivee.Id == s1.Id));
                        
                        Console.Write(connexionExiste ? "  X  " : "  -  ");
                    }
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine("\n=== Fin du graphe ===");
        }

        static void ComparerAlgorithmes(LivinParis.Models.Trajets.ReseauMetro reseau)
        {
            Console.WriteLine("\n=== Comparaison des algorithmes ===");
            Console.WriteLine("Stations disponibles:");
            foreach (var station in reseau.Stations.Values)
            {
                Console.WriteLine($"- {station.Nom}");
            }

            Console.Write("\nStation de départ: ");
            var depart = Console.ReadLine();
            Console.Write("Station d'arrivée: ");
            var arrivee = Console.ReadLine();

            var stationDepart = reseau.RechercherStationParNom(depart);
            var stationArrivee = reseau.RechercherStationParNom(arrivee);

            if (stationDepart == null || stationArrivee == null)
            {
                Console.WriteLine("Une ou plusieurs stations n'ont pas été trouvées.");
                return;
            }

            Console.WriteLine($"\nComparaison des algorithmes pour le trajet de {stationDepart.Nom} à {stationArrivee.Nom}");
            Console.WriteLine("=====================================================================");

            /// Dijkstra
            var startTime = DateTime.Now;
            var cheminDijkstra = reseau.Graphe.Dijkstra(stationDepart, stationArrivee);
            var dijkstraTime = (DateTime.Now - startTime).TotalMilliseconds;
            var distanceDijkstra = reseau.Graphe.CalculerDistanceTotale(cheminDijkstra);

            /// Bellman-Ford
            startTime = DateTime.Now;
            var cheminBellmanFord = reseau.Graphe.BellmanFord(stationDepart, stationArrivee);
            var bellmanFordTime = (DateTime.Now - startTime).TotalMilliseconds;
            var distanceBellmanFord = reseau.Graphe.CalculerDistanceTotale(cheminBellmanFord);

            /// Floyd-Warshall
            startTime = DateTime.Now;
            var cheminFloydWarshall = reseau.Graphe.FloydWarshall(stationDepart, stationArrivee);
            var floydWarshallTime = (DateTime.Now - startTime).TotalMilliseconds;
            var distanceFloydWarshall = reseau.Graphe.CalculerDistanceTotale(cheminFloydWarshall);

            /// Afficher les résultats
            Console.WriteLine("\nRésultats:");
            Console.WriteLine($"Dijkstra      : {dijkstraTime:F2} ms, {distanceDijkstra:F2} km, {cheminDijkstra.Count} stations");
            Console.WriteLine($"Bellman-Ford  : {bellmanFordTime:F2} ms, {distanceBellmanFord:F2} km, {cheminBellmanFord.Count} stations");
            Console.WriteLine($"Floyd-Warshall: {floydWarshallTime:F2} ms, {distanceFloydWarshall:F2} km, {cheminFloydWarshall.Count} stations");

            /// Vérifier si les chemins sont identiques
            bool cheminIdentique = 
                cheminDijkstra.Count == cheminBellmanFord.Count && 
                cheminBellmanFord.Count == cheminFloydWarshall.Count &&
                cheminDijkstra.SequenceEqual(cheminBellmanFord) && 
                cheminBellmanFord.SequenceEqual(cheminFloydWarshall);

            Console.WriteLine($"\nLes algorithmes ont trouvé {(cheminIdentique ? "le même" : "des chemins différents")}.");

            /// Afficher le meilleur algorithme en termes de performance
            Console.WriteLine("\nClassement par performance:");
            var performances = new[] 
            {
                (Algo: "Dijkstra", Temps: dijkstraTime),
                (Algo: "Bellman-Ford", Temps: bellmanFordTime),
                (Algo: "Floyd-Warshall", Temps: floydWarshallTime)
            }.OrderBy(p => p.Temps).ToList();

            for (int i = 0; i < performances.Count; i++)
            {
                Console.WriteLine($"{i+1}. {performances[i].Algo}: {performances[i].Temps:F2} ms");
            }

            /// Afficher le chemin trouvé par Dijkstra
            Console.WriteLine("\nChemin trouvé (Dijkstra):");
            foreach (var station in cheminDijkstra)
            {
                Console.WriteLine($"- {station.Nom}");
            }

            /// Visualiser le chemin
            AfficherGrapheChemin(cheminDijkstra);

            /// Générer une visualisation graphique du trajet
            Console.WriteLine("\nGénération d'une visualisation graphique du trajet...");
            var visualisation = new VisualisationReseau(reseau);
            visualisation.DessinerTrajet(cheminDijkstra);
            
            /// Créer un dossier pour les visualisations si nécessaire
            string dossierVisualisation = "Visualisations";
            if (!Directory.Exists(dossierVisualisation))
            {
                Directory.CreateDirectory(dossierVisualisation);
            }
            
            /// Nom du fichier avec date et heure pour éviter les écrasements
            string nomFichier = $"{dossierVisualisation}/trajet_{stationDepart.Nom}_vers_{stationArrivee.Nom}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            visualisation.SauvegarderImage(nomFichier);
            Console.WriteLine($"Visualisation du trajet sauvegardée dans : {nomFichier}");
        }
    }
}
