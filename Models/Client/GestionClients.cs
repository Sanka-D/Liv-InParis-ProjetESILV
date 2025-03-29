using System;
using System.Collections.Generic;
using System.Linq;

namespace LivinParis.Models.Client
{
    public class GestionClients
    {
        private static List<Client> _clients = new List<Client>();

        // Ajouter un client
        public static void AjouterClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Le client ne peut pas être null");

            _clients.Add(client);
        }

        // Modifier un client
        public static void ModifierClient(string identifiant, Client nouveauClient)
        {
            if (nouveauClient == null)
                throw new ArgumentNullException(nameof(nouveauClient), "Le client ne peut pas être null");

            var clientExistant = _clients.FirstOrDefault(c => c.Identifiant == identifiant);
            if (clientExistant == null)
                throw new Exception("Client non trouvé");

            clientExistant.Nom = nouveauClient.Nom;
            clientExistant.Prenom = nouveauClient.Prenom;
            clientExistant.Adresse = nouveauClient.Adresse;
            clientExistant.Telephone = nouveauClient.Telephone;
            clientExistant.Email = nouveauClient.Email;
            clientExistant.EstEntreprise = nouveauClient.EstEntreprise;
            clientExistant.NomEntreprise = nouveauClient.NomEntreprise;
            clientExistant.ReferentEntreprise = nouveauClient.ReferentEntreprise;
        }

        // Supprimer un client
        public static void SupprimerClient(string identifiant)
        {
            var client = _clients.FirstOrDefault(c => c.Identifiant == identifiant);
            if (client == null)
                throw new Exception("Client non trouvé");

            _clients.Remove(client);
        }

        // Obtenir tous les clients triés par nom et prénom
        public static List<Client> ObtenirClientsParOrdreAlphabetique()
        {
            return _clients
                .OrderBy(c => c.Nom)
                .ThenBy(c => c.Prenom)
                .ToList();
        }

        // Obtenir tous les clients triés par rue
        public static List<Client> ObtenirClientsParRue()
        {
            return _clients
                .OrderBy(c => c.Adresse)
                .ToList();
        }

        // Obtenir tous les clients triés par montant des achats
        public static List<Client> ObtenirClientsParMontantAchats()
        {
            return _clients
                .OrderByDescending(c => c.MontantAchats)
                .ToList();
        }

        // Afficher les clients selon le mode de tri choisi
        public static void AfficherClients(string modeTri = "alphabetique")
        {
            List<Client> clientsTries;

            switch (modeTri.ToLower())
            {
                case "rue":
                    clientsTries = ObtenirClientsParRue();
                    Console.WriteLine("\nClients triés par rue :");
                    break;
                case "montant":
                    clientsTries = ObtenirClientsParMontantAchats();
                    Console.WriteLine("\nClients triés par montant des achats :");
                    break;
                default:
                    clientsTries = ObtenirClientsParOrdreAlphabetique();
                    Console.WriteLine("\nClients triés par ordre alphabétique :");
                    break;
            }

            foreach (var client in clientsTries)
            {
                if (client.EstEntreprise)
                {
                    Console.WriteLine($"Entreprise: {client.NomEntreprise}");
                    Console.WriteLine($"Référent: {client.Prenom} {client.Nom}");
                }
                else
                {
                    Console.WriteLine($"Client: {client.Prenom} {client.Nom}");
                }
                Console.WriteLine($"Adresse: {client.Adresse}");
                Console.WriteLine($"Contact: {client.Email} - {client.Telephone}");
                Console.WriteLine($"Montant total des achats: {client.MontantAchats:C2}");
                Console.WriteLine("----------------------------------------");
            }
        }
    }
} 