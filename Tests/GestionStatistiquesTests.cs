using System;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Commande;
using LivinParis.Models.Statistiques;

namespace LivinParis.Tests
{
    public class GestionStatistiquesTests
    {
        public void Test_Statistiques_Globales()
        {
            // Arrange
            var client1 = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var client2 = new Client("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@email.com");
            var cuisinier = new Cuisinier("Petit", "Pierre", TestUtils.StationTest, "1122334455", "pierre@cuisine.com");

            var commande1 = GestionCommandes.CreerCommande(client1, cuisinier, DateTime.Now);
            var commande2 = GestionCommandes.CreerCommande(client2, cuisinier, DateTime.Now);

            // Act
            var statistiques = GestionStatistiques.ObtenirStatistiquesGlobales();

            // Assert
            if (statistiques.NombreTotalCommandes < 2) throw new Exception("Le nombre total de commandes n'est pas correct");
        }

        public void Test_Statistiques_Par_Client()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Petit", "Pierre", TestUtils.StationTest, "1122334455", "pierre@cuisine.com");

            var commande1 = GestionCommandes.CreerCommande(client, cuisinier, DateTime.Now);
            var commande2 = GestionCommandes.CreerCommande(client, cuisinier, DateTime.Now);

            // Act
            var statistiques = GestionStatistiques.ObtenirStatistiquesParClient(client.Identifiant);

            // Assert
            if (statistiques.NombreCommandes < 2) throw new Exception("Le nombre de commandes du client n'est pas correct");
        }

        public void Test_Statistiques_Par_Cuisinier()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Petit", "Pierre", TestUtils.StationTest, "1122334455", "pierre@cuisine.com");

            var commande1 = GestionCommandes.CreerCommande(client, cuisinier, DateTime.Now);
            var commande2 = GestionCommandes.CreerCommande(client, cuisinier, DateTime.Now);

            // Act
            var statistiques = GestionStatistiques.ObtenirStatistiquesParCuisinier(cuisinier.Identifiant);

            // Assert
            if (statistiques.NombreCommandes < 2) throw new Exception("Le nombre de commandes du cuisinier n'est pas correct");
        }
    }
} 