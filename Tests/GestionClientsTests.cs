using System;
using System.Linq;
using LivinParis.Models.Client;

namespace LivinParis.Tests
{
    public class GestionClientsTests
    {
        public void Test_Ajouter_Client()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");

            // Act
            GestionClients.AjouterClient(client);
            var clients = GestionClients.ObtenirClientsParOrdreAlphabetique();

            // Assert
            if (!clients.Contains(client)) throw new Exception("Le client n'a pas été ajouté");
        }

        public void Test_Modifier_Client()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            GestionClients.AjouterClient(client);

            var nouveauClient = new Client("Dupont", "Jean", TestUtils.StationTest, "9876543210", "jean.dupont@email.com");

            // Act
            GestionClients.ModifierClient(client.Identifiant, nouveauClient);
            var clients = GestionClients.ObtenirClientsParOrdreAlphabetique();
            var clientModifie = clients.FirstOrDefault(c => c.Identifiant == client.Identifiant);

            // Assert
            if (clientModifie == null) throw new Exception("Le client n'a pas été trouvé");
            if (clientModifie.Station != TestUtils.StationTest) throw new Exception("La station n'a pas été modifiée");
            if (clientModifie.Telephone != "9876543210") throw new Exception("Le téléphone n'a pas été modifié");
            if (clientModifie.Email != "jean.dupont@email.com") throw new Exception("L'email n'a pas été modifié");
        }

        public void Test_Supprimer_Client()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            GestionClients.AjouterClient(client);

            // Act
            GestionClients.SupprimerClient(client.Identifiant);
            var clients = GestionClients.ObtenirClientsParOrdreAlphabetique();

            // Assert
            if (clients.Contains(client)) throw new Exception("Le client n'a pas été supprimé");
        }

        public void Test_Tri_Clients()
        {
            // Arrange
            var client1 = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var client2 = new Client("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@email.com");
            var client3 = new Client("Albert", "Paul", TestUtils.StationTest, "1122334455", "paul@email.com");

            client1.AjouterAchat(100);
            client2.AjouterAchat(300);
            client3.AjouterAchat(200);

            GestionClients.AjouterClient(client1);
            GestionClients.AjouterClient(client2);
            GestionClients.AjouterClient(client3);

            // Test tri alphabétique
            var clientsAlpha = GestionClients.ObtenirClientsParOrdreAlphabetique();
            if (clientsAlpha[0].Nom != "Albert") throw new Exception("Le tri alphabétique n'est pas correct");

            // Test tri par station
            var clientsStation = GestionClients.ObtenirClientsParStation();
            if (clientsStation[0].Station != TestUtils.StationTest) throw new Exception("Le tri par station n'est pas correct");

            // Test tri par montant
            var clientsMontant = GestionClients.ObtenirClientsParMontantAchats();
            if (clientsMontant[0].MontantAchats != 300) throw new Exception("Le tri par montant n'est pas correct");
        }
    }
} 