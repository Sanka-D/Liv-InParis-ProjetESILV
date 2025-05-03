using Microsoft.VisualStudio.TestTools.UnitTesting;
using LivinParis.Models;
using System;
using System.Collections.Generic;

namespace LivinParis.Tests
{
    /// <summary>
    /// Test class for client management functionality.
    /// </summary>
    [TestClass]
    public class GestionClientsTests
    {
        private GestionClients gestionClients;
        private Client client;

        /// <summary>
        /// Initializes test data before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            gestionClients = new GestionClients();
            client = new Client
            {
                Id = 1,
                Nom = "Dupont",
                Prenom = "Jean",
                Email = "jean.dupont@email.com",
                Telephone = "0123456789",
                Adresse = "123 rue de Paris",
                Preferences = "Végétarien"
            };
        }

        /// <summary>
        /// Tests adding a new client.
        /// </summary>
        [TestMethod]
        public void TestAjouterClient()
        {
            var result = gestionClients.AjouterClient(client);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests retrieving a client by ID.
        /// </summary>
        [TestMethod]
        public void TestObtenirClient()
        {
            gestionClients.AjouterClient(client);
            var clientObtenu = gestionClients.ObtenirClient(1);
            Assert.IsNotNull(clientObtenu);
            Assert.AreEqual(client.Nom, clientObtenu.Nom);
        }

        /// <summary>
        /// Tests updating client information.
        /// </summary>
        [TestMethod]
        public void TestModifierClient()
        {
            gestionClients.AjouterClient(client);
            client.Nom = "Martin";
            var result = gestionClients.ModifierClient(client);
            Assert.IsTrue(result);
            var clientModifie = gestionClients.ObtenirClient(1);
            Assert.AreEqual("Martin", clientModifie.Nom);
        }

        /// <summary>
        /// Tests deleting a client.
        /// </summary>
        [TestMethod]
        public void TestSupprimerClient()
        {
            gestionClients.AjouterClient(client);
            var result = gestionClients.SupprimerClient(1);
            Assert.IsTrue(result);
            var clientSupprime = gestionClients.ObtenirClient(1);
            Assert.IsNull(clientSupprime);
        }

        /// <summary>
        /// Tests retrieving all clients.
        /// </summary>
        [TestMethod]
        public void TestObtenirTousLesClients()
        {
            gestionClients.AjouterClient(client);
            var clients = gestionClients.ObtenirTousLesClients();
            Assert.IsNotNull(clients);
            Assert.AreEqual(1, clients.Count);
        }
    }
} 