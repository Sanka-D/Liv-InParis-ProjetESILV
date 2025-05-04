using System;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Commande;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivinParis.Tests
{
    /// <summary>
    /// Test class for order management functionality.
    /// </summary>
    [TestClass]
    public class GestionCommandesTests
    {
        private IGestionCommandes gestionCommandes;
        private Commande commande;
        private Client client;
        private Plat plat;

        /// <summary>
        /// Initializes test data before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            gestionCommandes = GestionCommandes.Instance;
            client = new Client
            {
                Id = 1,
                Nom = "Dupont",
                Prenom = "Jean"
            };
            plat = new Plat
            {
                Id = 1,
                Nom = "Pizza",
                Prix = 15.99
            };
            commande = new Commande
            {
                Id = 1,
                ClientId = client.Id,
                PlatId = plat.Id,
                Quantite = 2,
                DateCommande = DateTime.Now,
                Statut = StatutCommande.EnAttente
            };
        }

        /// <summary>
        /// Tests creating a new order.
        /// </summary>
        [TestMethod]
        public void TestCreerCommande()
        {
            var result = gestionCommandes.CreerCommande(commande);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests retrieving an order by ID.
        /// </summary>
        [TestMethod]
        public void TestObtenirCommande()
        {
            gestionCommandes.CreerCommande(commande);
            var commandeObtenue = gestionCommandes.ObtenirCommande(1);
            Assert.IsNotNull(commandeObtenue);
            Assert.AreEqual(commande.ClientId, commandeObtenue.ClientId);
        }

        /// <summary>
        /// Tests updating order status.
        /// </summary>
        [TestMethod]
        public void TestModifierStatutCommande()
        {
            gestionCommandes.CreerCommande(commande);
            var result = gestionCommandes.ModifierStatutCommande(1, StatutCommande.EnPreparation);
            Assert.IsTrue(result);
            var commandeModifiee = gestionCommandes.ObtenirCommande(1);
            Assert.AreEqual(StatutCommande.EnPreparation, commandeModifiee.Statut);
        }

        /// <summary>
        /// Tests canceling an order.
        /// </summary>
        [TestMethod]
        public void TestAnnulerCommande()
        {
            gestionCommandes.CreerCommande(commande);
            var result = gestionCommandes.AnnulerCommande(1);
            Assert.IsTrue(result);
            var commandeAnnulee = gestionCommandes.ObtenirCommande(1);
            Assert.AreEqual(StatutCommande.Annulee, commandeAnnulee.Statut);
        }

        /// <summary>
        /// Tests retrieving all orders.
        /// </summary>
        [TestMethod]
        public void TestObtenirToutesLesCommandes()
        {
            gestionCommandes.CreerCommande(commande);
            var commandes = gestionCommandes.ObtenirToutesLesCommandes();
            Assert.IsNotNull(commandes);
            Assert.AreEqual(1, commandes.Count);
        }

        [TestMethod]
        public void Test_Creation_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;

            // Act
            var commande = gestionCommandes.CreerCommande(client, cuisinier, date);

            // Assert
            Assert.AreEqual(client.Id, commande.ClientId, "Le client n'est pas correct");
            Assert.AreEqual(cuisinier.Id, commande.CuisinierId, "Le cuisinier n'est pas correct");
            Assert.AreEqual(date.Date, commande.DateCommande.Date, "La date n'est pas correcte");
        }

        [TestMethod]
        public void Test_Modification_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;
            var commande = gestionCommandes.CreerCommande(client, cuisinier, date);

            var nouveauCuisinier = new Cuisinier("Petit", "Pierre", TestUtils.StationTest, "1122334455", "pierre@cuisine.com");

            // Act
            gestionCommandes.AssignerCuisinier(commande.Id, nouveauCuisinier.Id);

            // Assert
            var commandeModifiee = gestionCommandes.ObtenirCommande(commande.Id);
            Assert.AreEqual(nouveauCuisinier.Id, commandeModifiee.CuisinierId, "Le cuisinier n'a pas été modifié");
        }

        [TestMethod]
        public void Test_Suppression_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;
            var commande = gestionCommandes.CreerCommande(client, cuisinier, date);

            // Act
            gestionCommandes.SupprimerCommande(commande.Id);

            // Assert
            Assert.ThrowsException<Exception>(() => gestionCommandes.ObtenirCommande(commande.Id));
        }

        [TestMethod]
        public void Test_Simulation_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var date = DateTime.Now;
            var commande = gestionCommandes.CreerCommande(client, cuisinier, date);

            // Act
            gestionCommandes.AssignerCuisinier(commande.Id, cuisinier.Id);
            gestionCommandes.DemarrerPreparation(commande.Id);
            gestionCommandes.DemarrerLivraison(commande.Id);
            gestionCommandes.TerminerCommande(commande.Id);

            // Assert
            var commandeFinale = gestionCommandes.ObtenirCommande(commande.Id);
            Assert.AreEqual(StatutCommande.Terminee, commandeFinale.Statut, "La commande n'est pas terminée");
        }
    }
} 