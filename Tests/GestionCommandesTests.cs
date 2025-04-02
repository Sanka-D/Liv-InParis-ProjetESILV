using System;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Commande;

namespace LivinParis.Tests
{
    public class GestionCommandesTests
    {
        public void Test_Creation_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;

            // Act
            var commande = GestionCommandes.CreerCommande(client, cuisinier, date);

            // Assert
            if (commande.Client != client) throw new Exception("Le client n'est pas correct");
            if (commande.Cuisinier != cuisinier) throw new Exception("Le cuisinier n'est pas correct");
            if (commande.Date != date) throw new Exception("La date n'est pas correcte");
        }

        public void Test_Modification_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;
            var commande = GestionCommandes.CreerCommande(client, cuisinier, date);

            var nouveauCuisinier = new Cuisinier("Petit", "Pierre", TestUtils.StationTest, "1122334455", "pierre@cuisine.com");

            // Act
            GestionCommandes.AssignerCuisinier(commande.Identifiant, nouveauCuisinier.Identifiant);

            // Assert
            var commandeModifiee = GestionCommandes.ObtenirCommande(commande.Identifiant);
            if (commandeModifiee.Cuisinier != nouveauCuisinier) throw new Exception("Le cuisinier n'a pas été modifié");
        }

        public void Test_Suppression_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;
            var commande = GestionCommandes.CreerCommande(client, cuisinier, date);

            // Act
            GestionCommandes.SupprimerCommande(commande.Identifiant);

            // Assert
            try
            {
                GestionCommandes.ObtenirCommande(commande.Identifiant);
                throw new Exception("La commande aurait dû être supprimée");
            }
            catch (Exception) { /* OK */ }
        }

        public void Test_Simulation_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;
            var commande = GestionCommandes.CreerCommande(client, cuisinier, date);

            // Act
            GestionCommandes.AssignerCuisinier(commande.Identifiant, cuisinier.Identifiant);
            GestionCommandes.DemarrerPreparation(commande.Identifiant);
            GestionCommandes.DemarrerLivraison(commande.Identifiant);
            GestionCommandes.TerminerCommande(commande.Identifiant);

            // Assert
            var commandeFinale = GestionCommandes.ObtenirCommande(commande.Identifiant);
            if (commandeFinale.Statut != StatutCommande.Terminee) throw new Exception("La commande n'est pas terminée");
        }
    }
} 