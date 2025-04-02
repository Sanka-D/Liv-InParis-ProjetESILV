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

            // Initialiser GestionClients et GestionCuisiniers avec le réseau de métro
            GestionClients.Initialiser(_reseauMetro);
            GestionCuisiniers.Initialiser(_reseauMetro);

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
                Console.WriteLine("5. Réinitialiser la liste des clients");
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
                    case "5":
                        ReinitialiserClients();
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        private static void ReinitialiserClients()
        {
            Console.WriteLine("\n=== RÉINITIALISATION DE LA LISTE DES CLIENTS ===");
            Console.WriteLine("Êtes-vous sûr de vouloir supprimer tous les clients ? (O/N)");
            var reponse = Console.ReadLine()?.ToUpper();

            if (reponse == "O")
            {
                try
                {
                    GestionClients.ReinitialiserClients();
                    Console.WriteLine("Liste des clients réinitialisée avec succès !");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur : {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Opération annulée.");
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
                Console.WriteLine("5. Voir les détails d'un cuisinier");
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
                    case "5":
                        VoirDetailsCuisinier();
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
                Console.WriteLine("3. Démarrer la préparation");
                Console.WriteLine("4. Démarrer la livraison");
                Console.WriteLine("5. Terminer une commande");
                Console.WriteLine("6. Annuler une commande");
                Console.WriteLine("7. Afficher les commandes");
                Console.WriteLine("8. Calculer le prix d'une commande");
                Console.WriteLine("9. Voir le trajet de livraison");
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
                        DemarrerPreparation();
                        break;
                    case "4":
                        DemarrerLivraison();
                        break;
                    case "5":
                        TerminerCommande();
                        break;
                    case "6":
                        AnnulerCommande();
                        break;
                    case "7":
                        AfficherCommandes();
                        break;
                    case "8":
                        CalculerPrixCommande();
                        break;
                    case "9":
                        VoirTrajetLivraison();
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
                Console.WriteLine("4. Afficher les statistiques par client");
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
                        AfficherStatistiquesParCuisinier();
                        break;
                    case "3":
                        AfficherStatistiquesParNationalite();
                        break;
                    case "4":
                        AfficherStatistiquesParClient();
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
            
            // Recherche de la station de départ
            Station stationDepart = null;
            while (stationDepart == null)
            {
                Console.WriteLine("\nEntrez le nom de la station de départ (ou 'q' pour quitter) :");
                var nomDepart = Console.ReadLine();
                
                if (nomDepart.ToLower() == "q")
                    return;

                var stationsDepart = _reseauMetro.RechercherStationsParNom(nomDepart);
                
                if (stationsDepart.Count == 0)
                {
                    Console.WriteLine("Aucune station trouvée avec ce nom. Veuillez réessayer.");
                    continue;
                }

                // Si plusieurs stations trouvées, afficher les options
                if (stationsDepart.Count > 1)
                {
                    Console.WriteLine("\nPlusieurs stations trouvées. Choisissez une station :");
                    for (int i = 0; i < stationsDepart.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {stationsDepart[i].Nom} (ID: {stationsDepart[i].Id})");
                    }
                    Console.Write("\nVotre choix (numéro) ou 'r' pour réessayer : ");
                    var choix = Console.ReadLine();
                    
                    if (choix.ToLower() == "r")
                        continue;

                    if (int.TryParse(choix, out int choixNum) && choixNum > 0 && choixNum <= stationsDepart.Count)
                    {
                        stationDepart = stationsDepart[choixNum - 1];
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        continue;
                    }
                }
                else
                {
                    stationDepart = stationsDepart[0];
                }
            }

            // Recherche de la station d'arrivée
            Station stationArrivee = null;
            while (stationArrivee == null)
            {
                Console.WriteLine("\nEntrez le nom de la station d'arrivée (ou 'q' pour quitter) :");
                var nomArrivee = Console.ReadLine();
                
                if (nomArrivee.ToLower() == "q")
                    return;

                var stationsArrivee = _reseauMetro.RechercherStationsParNom(nomArrivee);
                
                if (stationsArrivee.Count == 0)
                {
                    Console.WriteLine("Aucune station trouvée avec ce nom. Veuillez réessayer.");
                    continue;
                }

                // Si plusieurs stations trouvées, afficher les options
                if (stationsArrivee.Count > 1)
                {
                    Console.WriteLine("\nPlusieurs stations trouvées. Choisissez une station :");
                    for (int i = 0; i < stationsArrivee.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {stationsArrivee[i].Nom} (ID: {stationsArrivee[i].Id})");
                    }
                    Console.Write("\nVotre choix (numéro) ou 'r' pour réessayer : ");
                    var choix = Console.ReadLine();
                    
                    if (choix.ToLower() == "r")
                        continue;

                    if (int.TryParse(choix, out int choixNum) && choixNum > 0 && choixNum <= stationsArrivee.Count)
                    {
                        stationArrivee = stationsArrivee[choixNum - 1];
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        continue;
                    }
                }
                else
                {
                    stationArrivee = stationsArrivee[0];
                }
            }

            try
            {
                Console.WriteLine($"\nCalcul du trajet de {stationDepart.Nom} vers {stationArrivee.Nom}...");
                _reseauMetro.ComparerAlgorithmes(stationDepart.Id, stationArrivee.Id);
                var trajet = _reseauMetro.ObtenirTrajet(stationDepart.Id, stationArrivee.Id);

                // Créer la visualisation
                var visualisation = new VisualisationReseau(_reseauMetro);
                visualisation.DessinerTrajet(trajet);

                // Sauvegarder l'image
                var cheminImage = $"trajet_{stationDepart.Id}_{stationArrivee.Id}.png";
                visualisation.SauvegarderImage(cheminImage);
                Console.WriteLine($"\nVisualisation du trajet sauvegardée dans : {cheminImage}");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
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
            
            // Sélection de la station
            Station station = null;
            while (station == null)
            {
                Console.WriteLine("\nEntrez le nom de la station de métro (ou 'q' pour quitter) :");
                var nomStation = Console.ReadLine();
                
                if (nomStation.ToLower() == "q")
                    return;

                var stations = _reseauMetro.RechercherStationsParNom(nomStation);
                
                if (stations.Count == 0)
                {
                    Console.WriteLine("Aucune station trouvée avec ce nom. Veuillez réessayer.");
                    continue;
                }

                // Si plusieurs stations trouvées, afficher les options
                if (stations.Count > 1)
                {
                    Console.WriteLine("\nPlusieurs stations trouvées. Choisissez une station :");
                    for (int i = 0; i < stations.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {stations[i].Nom} ({stations[i].Adresse})");
                    }
                    Console.Write("\nVotre choix (numéro) ou 'r' pour réessayer : ");
                    var choix = Console.ReadLine();
                    
                    if (choix.ToLower() == "r")
                        continue;

                    if (int.TryParse(choix, out int choixNum) && choixNum > 0 && choixNum <= stations.Count)
                    {
                        station = stations[choixNum - 1];
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        continue;
                    }
                }
                else
                {
                    station = stations[0];
                }
            }

            Console.Write("Téléphone : ");
            var telephone = Console.ReadLine();
            Console.Write("Email : ");
            var email = Console.ReadLine();

            var client = new Models.Client.Client(nom, prenom, station, telephone, email);
            Console.Write("Définir un mot de passe : ");
            var motDePasse = Console.ReadLine();
            client.DefinirMotDePasse(motDePasse);

            GestionClients.AjouterClient(client);
            Authentification.EnregistrerUtilisateur(client, motDePasse);
            
            Console.WriteLine($"\nClient ajouté avec succès !");
            Console.WriteLine($"Votre identifiant de connexion : {client.Identifiant}");
            Console.WriteLine($"Votre mot de passe : {motDePasse}");
            Console.WriteLine("\nConservez ces informations pour vous connecter ultérieurement.");
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
                
                // Sélection de la nouvelle station
                Station nouvelleStation = null;
                while (nouvelleStation == null)
                {
                    Console.WriteLine("\nEntrez le nom de la nouvelle station de métro (ou 'q' pour quitter) :");
                    var nomStation = Console.ReadLine();
                    
                    if (nomStation.ToLower() == "q")
                        return;

                    var stations = _reseauMetro.RechercherStationsParNom(nomStation);
                    
                    if (stations.Count == 0)
                    {
                        Console.WriteLine("Aucune station trouvée avec ce nom. Veuillez réessayer.");
                        continue;
                    }

                    // Si plusieurs stations trouvées, afficher les options
                    if (stations.Count > 1)
                    {
                        Console.WriteLine("\nPlusieurs stations trouvées. Choisissez une station :");
                        for (int i = 0; i < stations.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {stations[i].Nom} ({stations[i].Adresse})");
                        }
                        Console.Write("\nVotre choix (numéro) ou 'r' pour réessayer : ");
                        var choix = Console.ReadLine();
                        
                        if (choix.ToLower() == "r")
                            continue;

                        if (int.TryParse(choix, out int choixNum) && choixNum > 0 && choixNum <= stations.Count)
                        {
                            nouvelleStation = stations[choixNum - 1];
                        }
                        else
                        {
                            Console.WriteLine("Choix invalide. Veuillez réessayer.");
                            continue;
                        }
                    }
                    else
                    {
                        nouvelleStation = stations[0];
                    }
                }

                client.Station = nouvelleStation;
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
            Console.Write("Nom du client : ");
            var nom = Console.ReadLine();
            Console.Write("Prénom du client : ");
            var prenom = Console.ReadLine();

            var clients = GestionClients.ObtenirClientsParOrdreAlphabetique()
                .Where(c => c.Nom.ToLower() == nom.ToLower() && c.Prenom.ToLower() == prenom.ToLower())
                .ToList();

            if (clients.Count == 0)
            {
                Console.WriteLine("Aucun client trouvé avec ce nom et prénom.");
                return;
            }

            if (clients.Count > 1)
            {
                Console.WriteLine("\nPlusieurs clients trouvés. Choisissez le client à supprimer :");
                for (int i = 0; i < clients.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {clients[i].Prenom} {clients[i].Nom} (ID: {clients[i].Identifiant})");
                }
                Console.Write("\nVotre choix (numéro) : ");
                if (int.TryParse(Console.ReadLine(), out int choix) && choix > 0 && choix <= clients.Count)
                {
                    try
                    {
                        GestionClients.SupprimerClient(clients[choix - 1].Identifiant);
                        Console.WriteLine("Client supprimé avec succès !");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur : {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Choix invalide.");
                }
            }
            else
            {
                try
                {
                    GestionClients.SupprimerClient(clients[0].Identifiant);
                    Console.WriteLine("Client supprimé avec succès !");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur : {ex.Message}");
                }
            }
        }

        private static void AfficherClients()
        {
            while (true)
            {
                Console.WriteLine("\n=== LISTE DES CLIENTS ===");
                Console.WriteLine("1. Trier par ordre alphabétique");
                Console.WriteLine("2. Trier par rue");
                Console.WriteLine("3. Trier par montant des achats");
                Console.WriteLine("0. Retour au menu précédent");
                Console.Write("\nVotre choix : ");

                var choix = Console.ReadLine();
                if (choix == "0") break;

                switch (choix)
                {
                    case "1":
                        GestionClients.AfficherClients("alphabetique");
                        break;
                    case "2":
                        GestionClients.AfficherClients("rue");
                        break;
                    case "3":
                        GestionClients.AfficherClients("montant");
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }

                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }

        // Méthodes utilitaires pour les cuisiniers
        private static void AjouterCuisinier()
        {
            Console.WriteLine("\n=== AJOUT D'UN CUISINIER ===");
            Console.Write("Nom : ");
            var nom = Console.ReadLine();
            Console.Write("Prénom : ");
            var prenom = Console.ReadLine();
            
            // Sélection de la station
            Station station = null;
            while (station == null)
            {
                Console.WriteLine("\nEntrez le nom de la station de métro (ou 'q' pour quitter) :");
                var nomStation = Console.ReadLine();
                
                if (nomStation.ToLower() == "q")
                    return;

                var stations = _reseauMetro.RechercherStationsParNom(nomStation);
                
                if (stations.Count == 0)
                {
                    Console.WriteLine("Aucune station trouvée avec ce nom. Veuillez réessayer.");
                    continue;
                }

                // Si plusieurs stations trouvées, afficher les options
                if (stations.Count > 1)
                {
                    Console.WriteLine("\nPlusieurs stations trouvées. Choisissez une station :");
                    for (int i = 0; i < stations.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {stations[i].Nom} ({stations[i].Adresse})");
                    }
                    Console.Write("\nVotre choix (numéro) ou 'r' pour réessayer : ");
                    var choix = Console.ReadLine();
                    
                    if (choix.ToLower() == "r")
                        continue;

                    if (int.TryParse(choix, out int choixNum) && choixNum > 0 && choixNum <= stations.Count)
                    {
                        station = stations[choixNum - 1];
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        continue;
                    }
                }
                else
                {
                    station = stations[0];
                }
            }

            Console.Write("Téléphone : ");
            var telephone = Console.ReadLine();
            Console.Write("Email : ");
            var email = Console.ReadLine();

            var cuisinier = new Models.Cuisinier.Cuisinier(nom, prenom, station, telephone, email);
            Console.Write("Définir un mot de passe : ");
            var motDePasse = Console.ReadLine();
            cuisinier.DefinirMotDePasse(motDePasse);

            GestionCuisiniers.AjouterCuisinier(cuisinier);
            Authentification.EnregistrerUtilisateur(cuisinier, motDePasse);
            
            Console.WriteLine($"\nCuisinier ajouté avec succès !");
            Console.WriteLine($"Votre identifiant de connexion : {cuisinier.Identifiant}");
            Console.WriteLine($"Votre mot de passe : {motDePasse}");
            Console.WriteLine("\nConservez ces informations pour vous connecter ultérieurement.");
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
                
                // Sélection de la nouvelle station
                Station nouvelleStation = null;
                while (nouvelleStation == null)
                {
                    Console.WriteLine("\nEntrez le nom de la nouvelle station de métro (ou 'q' pour quitter) :");
                    var nomStation = Console.ReadLine();
                    
                    if (nomStation.ToLower() == "q")
                        return;

                    var stations = _reseauMetro.RechercherStationsParNom(nomStation);
                    
                    if (stations.Count == 0)
                    {
                        Console.WriteLine("Aucune station trouvée avec ce nom. Veuillez réessayer.");
                        continue;
                    }

                    // Si plusieurs stations trouvées, afficher les options
                    if (stations.Count > 1)
                    {
                        Console.WriteLine("\nPlusieurs stations trouvées. Choisissez une station :");
                        for (int i = 0; i < stations.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {stations[i].Nom} ({stations[i].Adresse})");
                        }
                        Console.Write("\nVotre choix (numéro) ou 'r' pour réessayer : ");
                        var choix = Console.ReadLine();
                        
                        if (choix.ToLower() == "r")
                            continue;

                        if (int.TryParse(choix, out int choixNum) && choixNum > 0 && choixNum <= stations.Count)
                        {
                            nouvelleStation = stations[choixNum - 1];
                        }
                        else
                        {
                            Console.WriteLine("Choix invalide. Veuillez réessayer.");
                            continue;
                        }
                    }
                    else
                    {
                        nouvelleStation = stations[0];
                    }
                }

                cuisinier.Station = nouvelleStation;
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

        private static void VoirDetailsCuisinier()
        {
            Console.WriteLine("\n=== DÉTAILS D'UN CUISINIER ===");
            Console.Write("ID du cuisinier : ");
            var id = Console.ReadLine();

            try
            {
                GestionCuisiniers.AfficherStatistiquesCuisinier(id);
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        // Méthodes utilitaires pour les commandes
        private static void CreerCommande()
        {
            Console.WriteLine("\n=== CRÉATION D'UNE COMMANDE ===");
            Console.Write("ID du client : ");
            var clientId = Console.ReadLine();
            Console.Write("ID du cuisinier : ");
            var cuisinierId = Console.ReadLine();

            try
            {
                var client = GestionClients.ObtenirClient(clientId);
                var cuisinier = GestionCuisiniers.ObtenirCuisinier(cuisinierId);
                var date = DateTime.Now;

                var commande = GestionCommandes.CreerCommande(client, cuisinier, date);
                Console.WriteLine($"Commande créée avec succès ! ID: {commande.Identifiant}");

                // Act
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

        private static void DemarrerPreparation()
        {
            Console.WriteLine("\n=== DÉMARRER LA PRÉPARATION ===");
            Console.Write("ID de la commande : ");
            var commandeId = Console.ReadLine();

            try
            {
                GestionCommandes.DemarrerPreparation(commandeId);
                Console.WriteLine("Préparation démarrée avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void DemarrerLivraison()
        {
            Console.WriteLine("\n=== DÉMARRER LA LIVRAISON ===");
            Console.Write("ID de la commande : ");
            var commandeId = Console.ReadLine();

            try
            {
                GestionCommandes.DemarrerLivraison(commandeId);
                Console.WriteLine("Livraison démarrée avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void AnnulerCommande()
        {
            Console.WriteLine("\n=== ANNULATION D'UNE COMMANDE ===");
            Console.Write("ID de la commande : ");
            var commandeId = Console.ReadLine();

            try
            {
                GestionCommandes.AnnulerCommande(commandeId);
                Console.WriteLine("Commande annulée avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void CalculerPrixCommande()
        {
            Console.WriteLine("\n=== CALCUL DU PRIX D'UNE COMMANDE ===");
            Console.Write("ID de la commande : ");
            var commandeId = Console.ReadLine();

            try
            {
                var prix = GestionCommandes.CalculerPrixCommande(commandeId);
                Console.WriteLine($"Prix total de la commande : {prix:C2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        private static void VoirTrajetLivraison()
        {
            Console.WriteLine("\n=== TRAJET DE LIVRAISON ===");
            Console.Write("ID de la commande : ");
            var commandeId = Console.ReadLine();

            try
            {
                var trajet = GestionCommandes.DeterminerCheminLivraison(commandeId);
                Console.WriteLine($"\nTrajet de livraison :\n{trajet}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }

        // Méthodes utilitaires pour les statistiques
        private static void AfficherStatistiquesParCuisinier()
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

        private static void AfficherStatistiquesParNationalite()
        {
            Console.WriteLine("\n=== STATISTIQUES PAR NATIONALITÉ ===");
            var commandesParNationalite = GestionStatistiques.ObtenirCommandesParNationalitePlats();
            foreach (var kvp in commandesParNationalite.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"- Cuisine {kvp.Key} : {kvp.Value} plat(s)");
            }
        }

        private static void AfficherStatistiquesParClient()
        {
            Console.WriteLine("\n=== STATISTIQUES PAR CLIENT ===");
            var clients = GestionClients.ObtenirClients();
            
            if (clients.Count == 0)
            {
                Console.WriteLine("Aucun client enregistré.");
                return;
            }

            foreach (var client in clients)
            {
                var stats = GestionStatistiques.ObtenirStatistiquesParClient(client.Identifiant);
                Console.WriteLine($"\nClient : {client.Prenom} {client.Nom}");
                Console.WriteLine($"Nombre de commandes : {stats.NombreCommandes}");
                Console.WriteLine($"Montant total : {stats.MontantTotal:C2}");
            }
        }
    }
} 