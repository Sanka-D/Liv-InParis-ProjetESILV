using System;
using LivinParis.Models.Client;

namespace LivinParis.Tests
{
    public class ClientTests
    {
        public void Test_Creation_Client_Particulier()
        {
            // Arrange
            string nom = "Dupont";
            string prenom = "Jean";
            string telephone = "0123456789";
            string email = "jean@email.com";

            // Act
            var client = new Client(nom, prenom, TestUtils.StationTest, telephone, email);

            // Assert
            if (client.Nom != nom) throw new Exception("Le nom n'est pas correct");
            if (client.Prenom != prenom) throw new Exception("Le prénom n'est pas correct");
            if (client.Station != TestUtils.StationTest) throw new Exception("La station n'est pas correcte");
            if (client.Telephone != telephone) throw new Exception("Le téléphone n'est pas correct");
            if (client.Email != email) throw new Exception("L'email n'est pas correct");
            if (client.EstEntreprise) throw new Exception("Le client ne devrait pas être une entreprise");
        }

        public void Test_Creation_Client_Entreprise()
        {
            // Arrange
            string nom = "Martin";
            string prenom = "Sophie";
            string telephone = "9876543210";
            string email = "sophie@entreprise.com";
            string nomEntreprise = "Entreprise SA";
            string referent = "Sophie Martin";

            // Act
            var client = new Client(nom, prenom, TestUtils.StationTest, telephone, email, true, nomEntreprise, referent);

            // Assert
            if (!client.EstEntreprise) throw new Exception("Le client devrait être une entreprise");
            if (client.NomEntreprise != nomEntreprise) throw new Exception("Le nom de l'entreprise n'est pas correct");
            if (client.ReferentEntreprise != referent) throw new Exception("Le référent n'est pas correct");
            if (client.Station != TestUtils.StationTest) throw new Exception("La station n'est pas correcte");
        }
    }
} 