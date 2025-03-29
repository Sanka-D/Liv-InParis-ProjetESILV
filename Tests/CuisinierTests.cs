using System;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Tests
{
    public class CuisinierTests
    {
        public void Test_Creation_Cuisinier()
        {
            // Arrange
            string nom = "Petit";
            string prenom = "Pierre";
            string adresse = "789 rue de Marseille";
            string telephone = "0123456789";
            string email = "pierre@cuisine.com";

            // Act
            var cuisinier = new Cuisinier(nom, prenom, adresse, telephone, email);

            // Assert
            if (cuisinier.Nom != nom) throw new Exception("Le nom n'est pas correct");
            if (cuisinier.Prenom != prenom) throw new Exception("Le prénom n'est pas correct");
            if (cuisinier.Adresse != adresse) throw new Exception("L'adresse n'est pas correcte");
            if (cuisinier.Telephone != telephone) throw new Exception("Le téléphone n'est pas correct");
            if (cuisinier.Email != email) throw new Exception("L'email n'est pas correct");
        }

        public void Test_ToString_Cuisinier()
        {
            // Arrange
            var cuisinier = new Cuisinier("Petit", "Pierre", "789 rue de Marseille", "0123456789", "pierre@cuisine.com");

            // Act
            string resultat = cuisinier.ToString();

            // Assert
            string attendu = "Cuisinier : Pierre Petit";
            if (resultat != attendu) throw new Exception($"La méthode ToString ne retourne pas le bon format. Attendu : {attendu}, Obtenu : {resultat}");
        }
    }
} 