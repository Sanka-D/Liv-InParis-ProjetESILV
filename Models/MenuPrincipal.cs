using System;
using System.Collections.Generic;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Commande;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Statistiques;
using LivinParis.Models.Trajets;

namespace LivinParis.Models
{
    public class MenuPrincipal
    {
        private static ReseauMetro _reseauMetro;
        private static bool _continuer = true;

        public static void Demarrer()
        {
            Console.WriteLine("=== LIVINPARIS - Application de Gestion de Livraison ===");
            Console.WriteLine("Chargement du réseau de métro...");
            _reseauMetro = ChargementXML.ChargerReseau("./Data/metro.xml");
            Console.WriteLine("Réseau de métro chargé avec succès !");

            while (_continuer)
            {
                AfficherMenuPrincipal();
            }
        }

        private static void AfficherMenuPrincipal()
        {
            Console.WriteLine("\n=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Gestion des Clients");
            Console.WriteLine("2. Gestion des Cuisiniers");
            Console.WriteLine("3. Gestion des Commandes");
            Console.WriteLine("4. Statistiques");
            Console.WriteLine("5. Planifier un Trajet");
            Console.WriteLine("6. Visualiser le Réseau");
            Console.WriteLine("0. Quitter");
            Console.Write("\nVotre choix : ");

            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    MenuClients();
                    break;
                case "2":
                    MenuCuisiniers();
                    break;
                case "3":
                    MenuCommandes();
                    break;
                case "4":
                    MenuStatistiques();
                    break;
                case "5":
                    MenuTrajets();
                    break;
                case "6":
                    VisualiserReseau();
                    break;
                case "0":
                    _continuer = false;
                    break;
                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    break;
            }
        }

        private static void MenuClients()
        {
            while (true)
            {
                Console.WriteLine("\n=== GESTION DES CLIENTS ===");
                Console.WriteLine("1. Ajouter un client");
                Console.WriteLine("2. Modifier un client");
                Console.WriteLine("3. Supprimer un client");
                Console.WriteLine("4. Afficher les clients");
                Console.WriteLine("0. Retour au menu principal");
                Console.Write("\nVotre choix : ");

                var choix = Console.ReadLine();
                if (choix == "0") break;

                switch (choix)
                {
                    case "1":
                        AjouterClient();
                        break;
                    case "2":
                        ModifierClient();
                        break;
                    case "3":
                        SupprimerClient();
                        break;
                    case "4":
                        AfficherClients();
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private static void MenuCuisiniers()
        {
            while (true)
            {
                Console.WriteLine("\n=== GESTION DES CUISINIERS ===");
                Console.WriteLine("1. Ajouter un cuisinier");
                Console.WriteLine("2. Modifier un cuisinier");
                Console.WriteLine("3. Supprimer un cuisinier");
                Console.WriteLine("4. Afficher les cuisiniers");
                Console.WriteLine("0. Retour au menu principal");
                Console.Write("\nVotre choix : ");

                var choix = Console.ReadLine();
                if (choix == "0") break;

                switch (choix)
                {
                    case "1":
                        AjouterCuisinier();
                        break;
                    case "2":
                        ModifierCuisinier();
                        break;
                    case "3":
                        SupprimerCuisinier();
                        break;
                    case "4":
                        AfficherCuisiniers();
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private static void MenuCommandes()
        {
            while (true)
            {
                Console.WriteLine("\n=== GESTION DES COMMANDES ===");
                Console.WriteLine("1. Créer une commande");
                Console.WriteLine("2. Assigner un cuisinier");
                Console.WriteLine("3. Terminer une commande");
                Console.WriteLine("4. Afficher les commandes");
                Console.WriteLine("0. Retour au menu principal");
                Console.Write("\nVotre choix : ");

                var choix = Console.ReadLine();
                if (choix == "0") break;

                switch (choix)
                {
                    case "1":
                        CreerCommande();
                        break;
                    case "2":
                        AssignerCuisinier();
                        break;
                    case "3":
                        TerminerCommande();
                        break;
                    case "4":
                        AfficherCommandes();
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private static void MenuStatistiques()
        {
            while (true)
            {
                Console.WriteLine("\n=== STATISTIQUES ===");
                Console.WriteLine("1. Afficher les statistiques générales");
                Console.WriteLine("2. Afficher les statistiques par cuisinier");
                Console.WriteLine("3. Afficher les statistiques par nationalité");
                Console.WriteLine("0. Retour au menu principal");
                Console.Write("\nVotre choix : ");

                var choix = Console.ReadLine();
                if (choix == "0") break;

                switch (choix)
                {
                    case "1":
                        GestionStatistiques.AfficherStatistiques();
                        break;
                    case "2":
                        AfficherStatistiquesCuisinier();
                        break;
                    case "3":
                        AfficherStatistiquesNationalite();
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private static void MenuTrajets()
        {
            Console.WriteLine("\n=== PLANIFICATION DE TRAJET ===");
            Console.WriteLine("Entrez l'ID de la station de départ :");
            if (int.TryParse(Console.ReadLine(), out int departId))
            {
                Console.WriteLine("Entrez l'ID de la station d'arrivée :");
                if (int.TryParse(Console.ReadLine(), out int arriveeId))
                {
                    try
                    {
                        _reseauMetro.ComparerAlgorithmes(departId, arriveeId);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Erreur : {ex.Message}");
                    }
                }
            }
        }

        private static void VisualiserReseau()
        {
            Console.WriteLine("\n=== VISUALISATION DU RÉSEAU ===");
            Console.WriteLine($"Nombre total de stations : {_reseauMetro.Stations.Count}");
            
            // Afficher les 10 premières stations comme exemple
            Console.WriteLine("\nExemple des 10 premières stations :");
            foreach (var station in _reseauMetro.Stations.Values.Take(10))
            {
                Console.WriteLine($"- {station.Nom} (ID: {station.Id})");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        // Méthodes utilitaires pour les clients
        private static void AjouterClient()
        {
            Console.WriteLine("\n=== AJOUT D'UN CLIENT ===");
            Console.Write("Nom : ");
            var nom = Console.ReadLine();
            Console.Write("Prénom : ");
            var prenom = Console.ReadLine();
            Console.Write("Adresse : ");
            var adresse = Console.ReadLine();
            Console.Write("Téléphone : ");
            var telephone = Console.ReadLine();
            Console.Write("Email : ");
            var email = Console.ReadLine();

            var client = new Models.Client.Client(nom, prenom, adresse, telephone, email);
            GestionClients.AjouterClient(client);
            Console.WriteLine("Client ajouté avec succès !");
        }

        private static void ModifierClient()
        {
            Console.WriteLine("\n=== MODIFICATION D'UN CLIENT ===");
            Console.Write("ID du client à modifier : ");
            var id = Console.ReadLine();

            var client = GestionClients.ObtenirClientsParOrdreAlphabetique()
                .FirstOrDefault(c => c.Identifiant == id);

            if (client != null)
            {
                Console.Write("Nouveau nom : ");
                client.Nom = Console.ReadLine();
                Console.Write("Nouveau prénom : ");
                client.Prenom = Console.ReadLine();
                Console.Write("Nouvelle adresse : ");
                client.Adresse = Console.ReadLine();
                Console.Write("Nouveau téléphone : ");
                client.Telephone = Console.ReadLine();
                Console.Write("Nouvel email : ");
                client.Email = Console.ReadLine();

                GestionClients.ModifierClient(id, client);
                Console.WriteLine("Client modifié avec succès !");
            }
            else
            {
                Console.WriteLine("Client non trouvé.");
            }
        }

        private static void SupprimerClient()
        {
            Console.WriteLine("\n=== SUPPRESSION D'UN CLIENT ===");
            Console.Write("ID du client à supprimer : ");
            var id = Console.ReadLine();

            try
            {
                GestionClients.SupprimerClient(id);
                Console.WriteLine("Client supprimé avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void AfficherClients()
        {
            Console.WriteLine("\n=== LISTE DES CLIENTS ===");
            GestionClients.AfficherClients();
        }

        // Méthodes utilitaires pour les cuisiniers
        private static void AjouterCuisinier()
        {
            Console.WriteLine("\n=== AJOUT D'UN CUISINIER ===");
            Console.Write("Nom : ");
            var nom = Console.ReadLine();
            Console.Write("Prénom : ");
            var prenom = Console.ReadLine();
            Console.Write("Adresse : ");
            var adresse = Console.ReadLine();
            Console.Write("Téléphone : ");
            var telephone = Console.ReadLine();
            Console.Write("Email : ");
            var email = Console.ReadLine();

            var cuisinier = new Models.Cuisinier.Cuisinier(nom, prenom, adresse, telephone, email);
            GestionCuisiniers.AjouterCuisinier(cuisinier);
            Console.WriteLine("Cuisinier ajouté avec succès !");
        }

        private static void ModifierCuisinier()
        {
            Console.WriteLine("\n=== MODIFICATION D'UN CUISINIER ===");
            Console.Write("ID du cuisinier à modifier : ");
            var id = Console.ReadLine();

            var cuisinier = GestionCuisiniers.ObtenirCuisinier(id);
            if (cuisinier != null)
            {
                Console.Write("Nouveau nom : ");
                cuisinier.Nom = Console.ReadLine();
                Console.Write("Nouveau prénom : ");
                cuisinier.Prenom = Console.ReadLine();
                Console.Write("Nouvelle adresse : ");
                cuisinier.Adresse = Console.ReadLine();
                Console.Write("Nouveau téléphone : ");
                cuisinier.Telephone = Console.ReadLine();
                Console.Write("Nouvel email : ");
                cuisinier.Email = Console.ReadLine();

                GestionCuisiniers.ModifierCuisinier(id, cuisinier);
                Console.WriteLine("Cuisinier modifié avec succès !");
            }
            else
            {
                Console.WriteLine("Cuisinier non trouvé.");
            }
        }

        private static void SupprimerCuisinier()
        {
            Console.WriteLine("\n=== SUPPRESSION D'UN CUISINIER ===");
            Console.Write("ID du cuisinier à supprimer : ");
            var id = Console.ReadLine();

            try
            {
                GestionCuisiniers.SupprimerCuisinier(id);
                Console.WriteLine("Cuisinier supprimé avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void AfficherCuisiniers()
        {
            Console.WriteLine("\n=== LISTE DES CUISINIERS ===");
            var cuisiniers = GestionCuisiniers.ObtenirCuisiniers();
            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"- {cuisinier.Prenom} {cuisinier.Nom} (ID: {cuisinier.Identifiant})");
            }
        }

        // Méthodes utilitaires pour les commandes
        private static void CreerCommande()
        {
            Console.WriteLine("\n=== CRÉATION D'UNE COMMANDE ===");
            Console.Write("ID du client : ");
            var clientId = Console.ReadLine();
            Console.Write("Adresse de livraison : ");
            var adresse = Console.ReadLine();

            try
            {
                var commande = GestionCommandes.CreerCommande(clientId, adresse);
                Console.WriteLine($"Commande créée avec succès ! ID: {commande.Identifiant}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void AssignerCuisinier()
        {
            Console.WriteLine("\n=== ASSIGNATION D'UN CUISINIER ===");
            Console.Write("ID de la commande : ");
            var commandeId = Console.ReadLine();
            Console.Write("ID du cuisinier : ");
            var cuisinierId = Console.ReadLine();

            try
            {
                GestionCommandes.AssignerCuisinier(commandeId, cuisinierId);
                Console.WriteLine("Cuisinier assigné avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void TerminerCommande()
        {
            Console.WriteLine("\n=== TERMINATION D'UNE COMMANDE ===");
            Console.Write("ID de la commande : ");
            var commandeId = Console.ReadLine();

            try
            {
                GestionCommandes.TerminerCommande(commandeId);
                Console.WriteLine("Commande terminée avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void AfficherCommandes()
        {
            Console.WriteLine("\n=== LISTE DES COMMANDES ===");
            foreach (var commande in GestionCommandes.ObtenirCommandes())
            {
                GestionCommandes.AfficherCommande(commande.Identifiant);
            }
        }

        // Méthodes utilitaires pour les statistiques
        private static void AfficherStatistiquesCuisinier()
        {
            Console.WriteLine("\n=== STATISTIQUES PAR CUISINIER ===");
            Console.Write("ID du cuisinier : ");
            var id = Console.ReadLine();

            try
            {
                GestionCuisiniers.AfficherStatistiquesCuisinier(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void AfficherStatistiquesNationalite()
        {
            Console.WriteLine("\n=== STATISTIQUES PAR NATIONALITÉ ===");
            var commandesParNationalite = GestionStatistiques.ObtenirCommandesParNationalitePlats();
            foreach (var kvp in commandesParNationalite.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"- Cuisine {kvp.Key} : {kvp.Value} plat(s)");
            }
        }
    }
} 