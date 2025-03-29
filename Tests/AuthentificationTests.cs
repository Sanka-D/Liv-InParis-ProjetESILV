using System;
using LivinParis.Models.Client;

namespace LivinParis.Tests
{
    public class AuthentificationTests
    {
        public void Test_Enregistrement_Et_Authentification()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", "123 rue de Paris", "0123456789", "jean@email.com");
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
            var client = new Client("Dupont", "Jean", "123 rue de Paris", "0123456789", "jean@email.com");
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