using Microsoft.VisualStudio.TestTools.UnitTesting;
using LivinParis.Models;
using System;
using System.Collections.Generic;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Tests
{
    /// <summary>
    /// Test class for cook management functionality.
    /// </summary>
    [TestClass]
    public class GestionCuisiniersTests
    {
        private GestionCuisiniers gestionCuisiniers;
        private Cuisinier cuisinier;

        /// <summary>
        /// Initializes test data before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            gestionCuisiniers = new GestionCuisiniers();
            cuisinier = new Cuisinier
            {
                Id = 1,
                Nom = "Dupont",
                Prenom = "Jean",
                Specialite = "Cuisine française",
                Experience = 5,
                Email = "jean.dupont@email.com",
                Telephone = "0123456789"
            };
        }

        /// <summary>
        /// Tests adding a new cook.
        /// </summary>
        [TestMethod]
        public void TestAjouterCuisinier()
        {
            var result = gestionCuisiniers.AjouterCuisinier(cuisinier);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests retrieving a cook by ID.
        /// </summary>
        [TestMethod]
        public void TestObtenirCuisinier()
        {
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            var cuisinierObtenu = gestionCuisiniers.ObtenirCuisinier(1);
            Assert.IsNotNull(cuisinierObtenu);
            Assert.AreEqual(cuisinier.Nom, cuisinierObtenu.Nom);
        }

        /// <summary>
        /// Tests updating cook information.
        /// </summary>
        [TestMethod]
        public void TestModifierCuisinier()
        {
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            cuisinier.Specialite = "Cuisine italienne";
            var result = gestionCuisiniers.ModifierCuisinier(cuisinier);
            Assert.IsTrue(result);
            var cuisinierModifie = gestionCuisiniers.ObtenirCuisinier(1);
            Assert.AreEqual("Cuisine italienne", cuisinierModifie.Specialite);
        }

        /// <summary>
        /// Tests deleting a cook.
        /// </summary>
        [TestMethod]
        public void TestSupprimerCuisinier()
        {
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            var result = gestionCuisiniers.SupprimerCuisinier(1);
            Assert.IsTrue(result);
            var cuisinierSupprime = gestionCuisiniers.ObtenirCuisinier(1);
            Assert.IsNull(cuisinierSupprime);
        }

        /// <summary>
        /// Tests retrieving all cooks.
        /// </summary>
        [TestMethod]
        public void TestObtenirTousLesCuisiniers()
        {
            gestionCuisiniers.AjouterCuisinier(cuisinier);
            var cuisiniers = gestionCuisiniers.ObtenirTousLesCuisiniers();
            Assert.IsNotNull(cuisiniers);
            Assert.AreEqual(1, cuisiniers.Count);
        }

        public void Test_Gestion_Plats()
        {
            // Arrange
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            GestionCuisiniers.AjouterCuisinier(cuisinier);

            // Act - Ajout de plats
            GestionCuisiniers.AjouterPlatRealise(cuisinier.Identifiant, "Coq au vin");
            GestionCuisiniers.AjouterPlatRealise(cuisinier.Identifiant, "Ratatouille");
            GestionCuisiniers.AjouterPlatRealise(cuisinier.Identifiant, "Coq au vin");

            // Act - Définition du plat du jour
            GestionCuisiniers.DefinirPlatDuJour(cuisinier.Identifiant, "Coq au vin");

            // Assert
            if (GestionCuisiniers.ObtenirPlatDuJour(cuisinier.Identifiant) != "Coq au vin")
                throw new Exception("Le plat du jour n'est pas correct");

            var platsFrequents = GestionCuisiniers.ObtenirPlatsParFrequence(cuisinier.Identifiant);
            if (platsFrequents.Count != 2) throw new Exception("Le nombre de plats différents n'est pas correct");
            if (platsFrequents[0] != "Coq au vin") throw new Exception("Le plat le plus fréquent n'est pas correct");
            if (platsFrequents[1] != "Ratatouille") throw new Exception("Le plat le moins fréquent n'est pas correct");
        }

        public void Test_Gestion_Clients_Servis()
        {
            // Arrange
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            GestionCuisiniers.AjouterCuisinier(cuisinier);

            // Act
            GestionCuisiniers.AjouterClientServi(cuisinier.Identifiant, client);
            var clientsServis = GestionCuisiniers.ObtenirClientsServis(cuisinier.Identifiant);

            // Assert
            if (!clientsServis.Contains(client)) throw new Exception("Le client n'a pas été ajouté aux clients servis");
        }
    }
} 