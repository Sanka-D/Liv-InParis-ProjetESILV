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
            string adresse = "123 rue de Paris";
            string telephone = "0123456789";
            string email = "jean@email.com";

            // Act
            var client = new Client(nom, prenom, adresse, telephone, email);

            // Assert
            if (client.Nom != nom) throw new Exception("Le nom n'est pas correct");
            if (client.Prenom != prenom) throw new Exception("Le prénom n'est pas correct");
            if (client.Adresse != adresse) throw new Exception("L'adresse n'est pas correcte");
            if (client.Telephone != telephone) throw new Exception("Le téléphone n'est pas correct");
            if (client.Email != email) throw new Exception("L'email n'est pas correct");
            if (client.EstEntreprise) throw new Exception("Le client ne devrait pas être une entreprise");
        }

        public void Test_Creation_Client_Entreprise()
        {
            // Arrange
            string nom = "Martin";
            string prenom = "Sophie";
            string adresse = "456 rue de Lyon";
            string telephone = "9876543210";
            string email = "sophie@entreprise.com";
            string nomEntreprise = "Entreprise SA";
            string referent = "Sophie Martin";

            // Act
            var client = new Client(nom, prenom, adresse, telephone, email, true, nomEntreprise, referent);

            // Assert
            if (!client.EstEntreprise) throw new Exception("Le client devrait être une entreprise");
            if (client.NomEntreprise != nomEntreprise) throw new Exception("Le nom de l'entreprise n'est pas correct");
            if (client.ReferentEntreprise != referent) throw new Exception("Le référent n'est pas correct");
        }
    }
} 