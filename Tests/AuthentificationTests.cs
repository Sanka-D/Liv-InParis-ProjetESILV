using Microsoft.VisualStudio.TestTools.UnitTesting;
using LivinParis.Models;
using System;
using LivinParis.Models.Client;

namespace LivinParis.Tests
{
    /// <summary>
    /// Test class for authentication functionality.
    /// </summary>
    [TestClass]
    public class AuthentificationTests
    {
        private Authentification auth;

        /// <summary>
        /// Initializes test data before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            auth = new Authentification();
        }

        /// <summary>
        /// Tests successful user login.
        /// </summary>
        [TestMethod]
        public void TestLoginSuccess()
        {
            var result = auth.Login("admin", "admin123");
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests failed user login with invalid credentials.
        /// </summary>
        [TestMethod]
        public void TestLoginFailure()
        {
            var result = auth.Login("invalid", "wrong");
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests user registration with valid data.
        /// </summary>
        [TestMethod]
        public void TestRegister()
        {
            var result = auth.Register("newuser", "password123", "user@example.com", UserRole.User);
            Assert.IsTrue(result);
        }

        public void Test_Enregistrement_Et_Authentification()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            string motDePasse = "motdepasse123";

            // Act
            Authentification.EnregistrerUtilisateur(client, motDePasse);
            var utilisateurAuthentifie = Authentification.Authentifier(client.Identifiant, motDePasse);

            // Assert
            if (utilisateurAuthentifie == null) throw new Exception("L'authentification a échoué");
            if (utilisateurAuthentifie != client) throw new Exception("L'utilisateur authentifié n'est pas le bon");
        }

        public void Test_Authentification_Echec()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", TestUtils.StationTest, "0123456789", "jean@email.com");
            string motDePasse = "motdepasse123";
            string mauvaisMotDePasse = "mauvaismdp";

            // Act
            Authentification.EnregistrerUtilisateur(client, motDePasse);
            var utilisateurAuthentifie = Authentification.Authentifier(client.Identifiant, mauvaisMotDePasse);

            // Assert
            if (utilisateurAuthentifie != null) throw new Exception("L'authentification aurait dû échouer");
        }
    }
} 