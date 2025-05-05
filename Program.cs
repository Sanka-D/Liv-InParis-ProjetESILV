using LivinParis.Data;
using LivinParis.Models;
using System;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;
using LivinParis.Models.Trajets;
using System.IO;
using SDColor = System.Drawing.Color;
using SDPointF = System.Drawing.PointF;
using SDRectangleF = System.Drawing.RectangleF;
using ImgColor = SixLabors.ImageSharp.Color;
using ImgPointF = SixLabors.ImageSharp.PointF;
using ImgRectangleF = SixLabors.ImageSharp.RectangleF;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using System.Security.Cryptography;
using System.Text;

namespace LivinParis
{
    /// <summary>
    /// Main program class for the Liv-In Paris application.
    /// </summary>
    class Program
    {
        private static DatabaseManager _dbManager;
        private static User _currentUser;

        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Tentative de connexion à la base de données...");
                Console.WriteLine("Serveur: 127.0.0.1:3306");
                Console.WriteLine("Base de données: livinparis");
                Console.WriteLine("Utilisateur: root");

                using (_dbManager = new DatabaseManager("127.0.0.1", "livinparis", "root", "270115"))
                {
                _dbManager.Connect();
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
                                if (_currentUser != null)
                                {
                            ViewDatabase();
                                }
                                else
                                {
                                    Console.WriteLine("Vous devez être connecté pour accéder à cette fonctionnalité.");
                                }
                            break;
                        case "4":
                                if (_currentUser != null)
                                {
                            GestionTrajets();
                                }
                                else
                                {
                                    Console.WriteLine("Vous devez être connecté pour accéder à cette fonctionnalité.");
                                }
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur détaillée: {ex.Message}");
                Console.WriteLine("\nVeuillez vérifier que:");
                Console.WriteLine("1. MySQL est en cours d'exécution sur le port 3306");
                Console.WriteLine("2. La base de données 'livinparis' existe");
                Console.WriteLine("3. Les identifiants sont corrects (root/root)");
                Console.WriteLine("\nAppuyez sur une touche pour quitter...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Handles user login with proper password hashing.
        /// </summary>
        static void Login()
        {
            Console.WriteLine("\n=== Connexion ===");
            Console.Write("Nom d'utilisateur: ");
            var username = Console.ReadLine();
            Console.Write("Mot de passe: ");
            var password = Console.ReadLine();

            var hashedPassword = HashPassword(password);
            var query = "SELECT * FROM users WHERE username = @username AND password = @password";
            var parameters = new Dictionary<string, object>
            {
                { "@username", username },
                { "@password", hashedPassword }
            };

            MySqlDataReader reader = null;
            try
            {
                reader = _dbManager.ExecuteReader(query, parameters);
                if (reader.Read())
                {
                    _currentUser = new User
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Username = reader["username"].ToString(),
                        Role = (UserRole)Enum.Parse(typeof(UserRole), reader["role"].ToString()),
                        Email = reader["email"].ToString()
                    };

                    Console.WriteLine($"Connexion réussie! Bienvenue {_currentUser.Username} ({_currentUser.Role})");
                    ShowUserMenu();
                }
                else
                {
                    Console.WriteLine("Nom d'utilisateur ou mot de passe incorrect.");
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Shows the appropriate menu based on user role.
        /// </summary>
        static void ShowUserMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Menu Utilisateur ===");
                Console.WriteLine("1. Visualiser les plats");
                Console.WriteLine("2. Créer une commande");
                Console.WriteLine("3. Calculer un trajet vers le restaurant");

                if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Manager)
                {
                    Console.WriteLine("4. Gérer les cuisiniers");
                    Console.WriteLine("5. Gérer les clients");
                    Console.WriteLine("6. Gérer les plats");
                }

                Console.WriteLine("7. Déconnexion");

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
                        if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Manager)
                            ManageCuisiniers();
                        break;
                    case "5":
                        if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Manager)
                            ManageClients();
                        break;
                    case "6":
                        if (_currentUser.Role == UserRole.Admin || _currentUser.Role == UserRole.Manager)
                            ManagePlats();
                        break;
                    case "7":
                        _currentUser = null;
                        return;
                    default:
                        Console.WriteLine("Option invalide");
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a new user account with proper validation.
        /// </summary>
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

            var hashedPassword = HashPassword(password);
            var query = "INSERT INTO users (username, password, role, email) VALUES (@username, @password, @role, @email)";
            var parameters = new Dictionary<string, object>
            {
                { "@username", username },
                { "@password", hashedPassword },
                { "@role", role.ToString() },
                { "@email", email }
            };

            try
            {
                _dbManager.ExecuteNonQuery(query, parameters);
                Console.WriteLine("Compte créé avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du compte: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays the database management menu with proper permissions.
        /// </summary>
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

        /// <summary>
        /// Displays the list of cooks with proper formatting.
        /// </summary>
        static void DisplayCuisiniers()
        {
            Console.WriteLine("\n=== Liste des cuisiniers ===");
            var query = "SELECT * FROM cuisiniers ORDER BY nom, prenom";
            MySqlDataReader reader = null;
            try
            {
                reader = _dbManager.ExecuteReader(query);
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
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Displays the list of clients with proper formatting.
        /// </summary>
        static void DisplayClients()
        {
            Console.WriteLine("\n=== Liste des clients ===");
            var query = "SELECT * FROM clients ORDER BY nom, prenom";
            MySqlDataReader reader = null;
            try
            {
                reader = _dbManager.ExecuteReader(query);
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
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Displays the list of dishes with proper formatting.
        /// </summary>
        static void DisplayPlats()
        {
            Console.WriteLine("\n=== Liste des plats ===");
            var query = @"SELECT p.*, c.nom as cuisinier_nom, c.prenom as cuisinier_prenom 
                         FROM plats p 
                         JOIN cuisiniers c ON p.cuisinier_id = c.id 
                         ORDER BY p.type, p.nom";
            MySqlDataReader reader = null;
            try
            {
                reader = _dbManager.ExecuteReader(query);
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
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Manages cooks (Admin/Manager only).
        /// </summary>
        static void ManageCuisiniers()
        {
            while (true)
            {
                Console.WriteLine("\n=== Gestion des Cuisiniers ===");
                Console.WriteLine("1. Afficher les cuisiniers");
                Console.WriteLine("2. Ajouter un cuisinier");
                Console.WriteLine("3. Modifier un cuisinier");
                Console.WriteLine("4. Supprimer un cuisinier");
                Console.WriteLine("5. Retour");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayCuisiniers();
                        break;
                    case "2":
                        AddCuisinier();
                        break;
                    case "3":
                        UpdateCuisinier();
                        break;
                    case "4":
                        DeleteCuisinier();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Option invalide");
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a new cook to the database.
        /// </summary>
        static void AddCuisinier()
            {
            Console.WriteLine("\n=== Ajouter un cuisinier ===");
            Console.Write("Nom: ");
            var nom = Console.ReadLine();
            Console.Write("Prénom: ");
            var prenom = Console.ReadLine();
            Console.Write("Spécialité: ");
            var specialite = Console.ReadLine();
            Console.Write("Expérience (années): ");
            var experience = int.Parse(Console.ReadLine());
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Téléphone: ");
            var telephone = Console.ReadLine();

            var query = @"INSERT INTO cuisiniers (nom, prenom, specialite, experience, email, telephone) 
                         VALUES (@nom, @prenom, @specialite, @experience, @email, @telephone)";
            var parameters = new Dictionary<string, object>
            {
                { "@nom", nom },
                { "@prenom", prenom },
                { "@specialite", specialite },
                { "@experience", experience },
                { "@email", email },
                { "@telephone", telephone }
            };

            try
            {
                _dbManager.ExecuteNonQuery(query, parameters);
                Console.WriteLine("Cuisinier ajouté avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du cuisinier: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing cook's information.
        /// </summary>
        static void UpdateCuisinier()
        {
            Console.WriteLine("\n=== Modifier un cuisinier ===");
            Console.Write("ID du cuisinier à modifier: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                {
                Console.WriteLine("ID invalide");
                return;
            }

            Console.Write("Nouveau nom (laissez vide pour ne pas modifier): ");
            var nom = Console.ReadLine();
            Console.Write("Nouveau prénom (laissez vide pour ne pas modifier): ");
            var prenom = Console.ReadLine();
            Console.Write("Nouvelle spécialité (laissez vide pour ne pas modifier): ");
            var specialite = Console.ReadLine();
            Console.Write("Nouvelle expérience (années, laissez vide pour ne pas modifier): ");
            var experienceStr = Console.ReadLine();
            Console.Write("Nouvel email (laissez vide pour ne pas modifier): ");
            var email = Console.ReadLine();
            Console.Write("Nouveau téléphone (laissez vide pour ne pas modifier): ");
            var telephone = Console.ReadLine();
                
            var updates = new List<string>();
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(nom))
            {
                updates.Add("nom = @nom");
                parameters.Add("@nom", nom);
            }
            if (!string.IsNullOrEmpty(prenom))
            {
                updates.Add("prenom = @prenom");
                parameters.Add("@prenom", prenom);
            }
            if (!string.IsNullOrEmpty(specialite))
            {
                updates.Add("specialite = @specialite");
                parameters.Add("@specialite", specialite);
            }
            if (!string.IsNullOrEmpty(experienceStr) && int.TryParse(experienceStr, out int experience))
            {
                updates.Add("experience = @experience");
                parameters.Add("@experience", experience);
            }
            if (!string.IsNullOrEmpty(email))
            {
                updates.Add("email = @email");
                parameters.Add("@email", email);
            }
            if (!string.IsNullOrEmpty(telephone))
            {
                updates.Add("telephone = @telephone");
                parameters.Add("@telephone", telephone);
            }

            if (updates.Count == 0)
                {
                Console.WriteLine("Aucune modification effectuée.");
                    return;
                }

            parameters.Add("@id", id);
            var query = $"UPDATE cuisiniers SET {string.Join(", ", updates)} WHERE id = @id";

            try
            {
                _dbManager.ExecuteNonQuery(query, parameters);
                Console.WriteLine("Cuisinier modifié avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du cuisinier: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a cook from the database.
        /// </summary>
        static void DeleteCuisinier()
                {
            Console.WriteLine("\n=== Supprimer un cuisinier ===");
            Console.Write("ID du cuisinier à supprimer: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalide");
                    return;
                }

            var query = "DELETE FROM cuisiniers WHERE id = @id";
            var parameters = new Dictionary<string, object>
            {
                { "@id", id }
            };

            try
            {
                _dbManager.ExecuteNonQuery(query, parameters);
                Console.WriteLine("Cuisinier supprimé avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression du cuisinier: {ex.Message}");
            }
        }

        /// <summary>
        /// Manages clients (Admin/Manager only).
        /// </summary>
        static void ManageClients()
        {
            while (true)
                {
                Console.WriteLine("\n=== Gestion des Clients ===");
                Console.WriteLine("1. Afficher les clients");
                Console.WriteLine("2. Ajouter un client");
                Console.WriteLine("3. Modifier un client");
                Console.WriteLine("4. Supprimer un client");
                Console.WriteLine("5. Retour");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayClients();
                        break;
                    case "2":
                        AddClient();
                        break;
                    case "3":
                        UpdateClient();
                        break;
                    case "4":
                        DeleteClient();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Option invalide");
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a new client to the database.
        /// </summary>
        static void AddClient()
        {
            Console.WriteLine("\n=== Ajouter un client ===");
            Console.Write("Nom: ");
            var nom = Console.ReadLine();
            Console.Write("Prénom: ");
            var prenom = Console.ReadLine();
            Console.Write("Adresse: ");
            var adresse = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Téléphone: ");
            var telephone = Console.ReadLine();
            Console.Write("Préférences: ");
            var preferences = Console.ReadLine();

            var query = $"INSERT INTO clients (nom, prenom, adresse, email, telephone, preferences) VALUES ('{nom}', '{prenom}', '{adresse}', '{email}', '{telephone}', '{preferences}')";

            try
            {
                _dbManager.ExecuteNonQuery(query);
                Console.WriteLine("Client ajouté avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du client: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing client's information.
        /// </summary>
        static void UpdateClient()
        {
            Console.WriteLine("\n=== Modifier un client ===");
            Console.Write("ID du client à modifier: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalide");
                return;
            }

            Console.Write("Nouveau nom (laissez vide pour ne pas modifier): ");
            var nom = Console.ReadLine();
            Console.Write("Nouveau prénom (laissez vide pour ne pas modifier): ");
            var prenom = Console.ReadLine();
            Console.Write("Nouvelle adresse (laissez vide pour ne pas modifier): ");
            var adresse = Console.ReadLine();
            Console.Write("Nouvel email (laissez vide pour ne pas modifier): ");
            var email = Console.ReadLine();
            Console.Write("Nouveau téléphone (laissez vide pour ne pas modifier): ");
            var telephone = Console.ReadLine();
            Console.Write("Nouvelles préférences (laissez vide pour ne pas modifier): ");
            var preferences = Console.ReadLine();

            var updates = new List<string>();

            if (!string.IsNullOrEmpty(nom))
                updates.Add($"nom = '{nom}'");
            if (!string.IsNullOrEmpty(prenom))
                updates.Add($"prenom = '{prenom}'");
            if (!string.IsNullOrEmpty(adresse))
                updates.Add($"adresse = '{adresse}'");
            if (!string.IsNullOrEmpty(email))
                updates.Add($"email = '{email}'");
            if (!string.IsNullOrEmpty(telephone))
                updates.Add($"telephone = '{telephone}'");
            if (!string.IsNullOrEmpty(preferences))
                updates.Add($"preferences = '{preferences}'");

            if (updates.Count == 0)
            {
                Console.WriteLine("Aucune modification effectuée.");
                return;
            }

            var query = $"UPDATE clients SET {string.Join(", ", updates)} WHERE id = {id}";

            try
            {
                _dbManager.ExecuteNonQuery(query);
                Console.WriteLine("Client modifié avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du client: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a client from the database.
        /// </summary>
        static void DeleteClient()
        {
            Console.WriteLine("\n=== Supprimer un client ===");
            Console.Write("ID du client à supprimer: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalide");
                return;
            }

            var query = $"DELETE FROM clients WHERE id = {id}";

            try
            {
                _dbManager.ExecuteNonQuery(query);
                Console.WriteLine("Client supprimé avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression du client: {ex.Message}");
            }
        }

        /// <summary>
        /// Manages dishes (Admin/Manager only).
        /// </summary>
        static void ManagePlats()
        {
            while (true)
            {
                Console.WriteLine("\n=== Gestion des Plats ===");
                Console.WriteLine("1. Afficher les plats");
                Console.WriteLine("2. Ajouter un plat");
                Console.WriteLine("3. Modifier un plat");
                Console.WriteLine("4. Supprimer un plat");
                Console.WriteLine("5. Retour");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
            DisplayPlats();
                        break;
                    case "2":
                        AddPlat();
                        break;
                    case "3":
                        UpdatePlat();
                        break;
                    case "4":
                        DeletePlat();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Option invalide");
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a new dish to the database.
        /// </summary>
        static void AddPlat()
        {
            Console.WriteLine("\n=== Ajouter un plat ===");
            Console.Write("Nom: ");
            var nom = Console.ReadLine();
            Console.Write("Description: ");
            var description = Console.ReadLine();
            Console.Write("Prix: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal prix))
            {
                Console.WriteLine("Prix invalide");
                return;
            }
            Console.WriteLine("Type (1: Entrée, 2: Plat principal, 3: Dessert): ");
            var typeChoice = Console.ReadLine();
            var type = typeChoice switch
            {
                "1" => "Entrée",
                "2" => "Plat principal",
                "3" => "Dessert",
                _ => "Plat principal"
            };
            Console.Write("ID du cuisinier: ");
            if (!int.TryParse(Console.ReadLine(), out int cuisinierId))
            {
                Console.WriteLine("ID de cuisinier invalide");
                return;
            }

            var query = $"INSERT INTO plats (nom, description, prix, type, cuisinier_id) VALUES ('{nom}', '{description}', {prix}, '{type}', {cuisinierId})";

            try
            {
                _dbManager.ExecuteNonQuery(query);
                Console.WriteLine("Plat ajouté avec succès!");
                }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du plat: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing dish's information.
        /// </summary>
        static void UpdatePlat()
        {
            Console.WriteLine("\n=== Modifier un plat ===");
            Console.Write("ID du plat à modifier: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalide");
                return;
            }

            Console.Write("Nouveau nom (laissez vide pour ne pas modifier): ");
            var nom = Console.ReadLine();
            Console.Write("Nouvelle description (laissez vide pour ne pas modifier): ");
            var description = Console.ReadLine();
            Console.Write("Nouveau prix (laissez vide pour ne pas modifier): ");
            var prixStr = Console.ReadLine();
            Console.WriteLine("Nouveau type (1: Entrée, 2: Plat principal, 3: Dessert, laissez vide pour ne pas modifier): ");
            var typeChoice = Console.ReadLine();
            Console.Write("Nouvel ID du cuisinier (laissez vide pour ne pas modifier): ");
            var cuisinierIdStr = Console.ReadLine();

            var updates = new List<string>();

            if (!string.IsNullOrEmpty(nom))
                updates.Add($"nom = '{nom}'");
            if (!string.IsNullOrEmpty(description))
                updates.Add($"description = '{description}'");
            if (!string.IsNullOrEmpty(prixStr) && decimal.TryParse(prixStr, out decimal prix))
                updates.Add($"prix = {prix}");
            if (!string.IsNullOrEmpty(typeChoice))
            {
                var type = typeChoice switch
            {
                    "1" => "Entrée",
                    "2" => "Plat principal",
                    "3" => "Dessert",
                    _ => "Plat principal"
                };
                updates.Add($"type = '{type}'");
            }
            if (!string.IsNullOrEmpty(cuisinierIdStr) && int.TryParse(cuisinierIdStr, out int cuisinierId))
                updates.Add($"cuisinier_id = {cuisinierId}");

            if (updates.Count == 0)
            {
                Console.WriteLine("Aucune modification effectuée.");
                return;
            }

            var query = $"UPDATE plats SET {string.Join(", ", updates)} WHERE id = {id}";

            try
            {
                _dbManager.ExecuteNonQuery(query);
                Console.WriteLine("Plat modifié avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du plat: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a dish from the database.
        /// </summary>
        static void DeletePlat()
        {
            Console.WriteLine("\n=== Supprimer un plat ===");
            Console.Write("ID du plat à supprimer: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID invalide");
                return;
            }

            var query = $"DELETE FROM plats WHERE id = {id}";

            try
            {
                _dbManager.ExecuteNonQuery(query);
                Console.WriteLine("Plat supprimé avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression du plat: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        static void CreateOrder()
        {
            Console.WriteLine("\n=== Création d'une commande ===");
            DisplayPlats();
            
            Console.Write("\nEntrez l'ID du plat que vous souhaitez commander: ");
            if (!int.TryParse(Console.ReadLine(), out int platId))
            {
                Console.WriteLine("ID de plat invalide");
                return;
            }

            Console.Write("Quantité: ");
            if (!int.TryParse(Console.ReadLine(), out int quantite))
            {
                Console.WriteLine("Quantité invalide");
                return;
            }

            // TODO: Implémenter la création de la commande dans la base de données
            Console.WriteLine("Commande créée avec succès!");
        }

        /// <summary>
        /// Hashes a password using SHA256.
        /// </summary>
        static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Handles route management.
        /// </summary>
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
                    Console.WriteLine("4. Coloration du graphe (Welsh-Powell), export et arborescence");
                    Console.WriteLine("5. Retour au menu principal");

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
                            AnalyseColorationEtArborescence(reseauMetro);
                            break;
                        case "5":
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

        /// <summary>
        /// Searches for the shortest path between two stations.
        /// </summary>
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

        /// <summary>
        /// Displays the path graph.
        /// </summary>
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

        /// <summary>
        /// Visualizes the metro network.
        /// </summary>
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

        /// <summary>
        /// Displays the network graph.
        /// </summary>
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

        /// <summary>
        /// Compares different path-finding algorithms.
        /// </summary>
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

        /// <summary>
        /// Analyzes graph coloring and arborescence.
        /// </summary>
        static void AnalyseColorationEtArborescence(LivinParis.Models.Trajets.ReseauMetro reseau)
        {
            Console.WriteLine("\n=== Analyse coloration (Welsh-Powell), export et arborescence (Chu-Liu/Edmonds) ===");
            var graphe = reseau.Graphe;
            
            // 1. Welsh-Powell coloring
            var coloring = GraphAlgorithms.WelshPowellColoring(reseau.Graphe);
            int minColors = coloring.Values.Distinct().Count();
            Console.WriteLine($"Nombre minimal de couleurs nécessaires : {minColors}");
            
            // 2. Bipartite check
            bool bipartite = GraphAlgorithms.IsBipartite(coloring);
            Console.WriteLine($"Le graphe est biparti ? {bipartite} (car {minColors} couleurs)");
            
            // 3. Planar check
            bool planar = GraphAlgorithms.IsPlanar(reseau.Graphe);
            Console.WriteLine($"Le graphe est planaire ? {planar} (m <= 3n-6)");
            
            // 4. Independent groups
            var groups = GraphAlgorithms.GetIndependentGroups(coloring);
            Console.WriteLine("Groupes indépendants (même couleur) :");
            foreach (var kvp in groups)
            {
                Console.WriteLine($"  Couleur {kvp.Key} : {string.Join(", ", kvp.Value.Select(s => s.ToString()))}");
            }
            
            // 5. Visualize colored graph
            var visualisation = new VisualisationReseau(reseau);
            string dossierVisualisation = "Visualisations";
            if (!Directory.Exists(dossierVisualisation)) Directory.CreateDirectory(dossierVisualisation);
            string nomFichier = $"{dossierVisualisation}/coloration_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            visualisation.DessinerColoration(coloring, nomFichier);
            Console.WriteLine($"Graphe coloré sauvegardé dans : {nomFichier}");
            
            // 6. Export to JSON/XML
            string jsonPath = $"{dossierVisualisation}/coloration_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string xmlPath = $"{dossierVisualisation}/coloration_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
            GraphAlgorithms.ExportToJson(reseau.Graphe, coloring, jsonPath);
            GraphAlgorithms.ExportToXml(reseau.Graphe, coloring, xmlPath);
            Console.WriteLine($"Export JSON : {jsonPath}");
            Console.WriteLine($"Export XML : {xmlPath}");
            
            // 7. Chu-Liu/Edmonds arborescence
            Console.WriteLine("\n--- Arborescence minimale (Chu-Liu/Edmonds) ---");
            // Build directed, weighted edges (use distance as weight)
            var nodes = reseau.Stations.Values.ToList();
            var edges = new List<(Station From, Station To, double Weight)>();
            foreach (var noeud in reseau.Graphe.Noeuds.Values)
            {
                foreach (var voisin in noeud.Voisins)
                {
                    edges.Add((noeud.Valeur, voisin.Key.Valeur, voisin.Value)); // Directed
                }
            }
            
            // Select a root (first cook or station)
            var root = nodes.FirstOrDefault();
            if (root == null)
            {
                Console.WriteLine("Aucune station trouvée pour l'arborescence.");
                return;
            }
            
            var arborescence = GraphAlgorithms.ChuLiuEdmonds(nodes, edges, root);
            Console.WriteLine($"Arborescence minimale à partir de {root.Nom} :");
            foreach (var arc in arborescence)
            {
                Console.WriteLine($"{arc.From.Nom} -> {arc.To.Nom} (poids: {arc.Weight:F2})");
            }
            
            // Visualisation de l'arborescence (optionnelle, simple)
            string arboFile = $"{dossierVisualisation}/arborescence_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            VisualiserArborescence(reseau, arborescence, arboFile);
            Console.WriteLine($"Arborescence visualisée dans : {arboFile}");
        }

        /// <summary>
        /// Visualizes the arborescence.
        /// </summary>
        static void VisualiserArborescence(LivinParis.Models.Trajets.ReseauMetro reseau, List<(Station From, Station To, double Weight)> arbo, string chemin)
        {
            var visualisation = new VisualisationReseau(reseau);
            using (var image = new Image<Rgba32>(1200, 800))
            {
                image.Mutate(x => x.Fill(ImgColor.White));
                
                // Dessiner les arêtes de l'arborescence
                foreach (var arc in arbo)
                {
                    var point1 = new ImgPointF((float)arc.From.Longitude, (float)arc.From.Latitude);
                    var point2 = new ImgPointF((float)arc.To.Longitude, (float)arc.To.Latitude);
                    point1 = visualisation.GetType().GetMethod("ConvertirCoordonnees", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .Invoke(visualisation, new object[] { arc.From.Latitude, arc.From.Longitude }) is ImgPointF p1 ? p1 : point1;
                    point2 = visualisation.GetType().GetMethod("ConvertirCoordonnees", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .Invoke(visualisation, new object[] { arc.To.Latitude, arc.To.Longitude }) is ImgPointF p2 ? p2 : point2;
                    var pen = new SolidPen(ImgColor.DarkGreen, 4f);
                    image.Mutate(x => x.DrawLine(pen, point1, point2));
                }
                
                // Dessiner les sommets
                foreach (var station in reseau.Stations.Values)
                {
                    var point = visualisation.GetType().GetMethod("ConvertirCoordonnees", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .Invoke(visualisation, new object[] { station.Latitude, station.Longitude }) is ImgPointF p ? p : new ImgPointF((float)station.Longitude, (float)station.Latitude);
                    var rect = new ImgRectangleF(point.X - 4, point.Y - 4, 8, 8);
                    image.Mutate(x => x.Fill(ImgColor.Blue, rect));
                }
                
                image.Save(chemin);
            }
        }

        /// <summary>
        /// Calculates and displays the route to the restaurant.
        /// </summary>
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
    }

    /// <summary>
    /// User class to store current user information.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// User role enumeration.
    /// </summary>
    public enum UserRole
    {
        Admin,
        Manager,
        User
    }
}
