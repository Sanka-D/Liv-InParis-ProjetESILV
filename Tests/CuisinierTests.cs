using System;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Tests
{
    public class CuisinierTests
    {
        public void Test_Creation_Cuisinier()
        {
            // Arrange
            string nom = "Martin";
            string prenom = "Sophie";
            string telephone = "9876543210";
            string email = "sophie@cuisine.com";

            // Act
            var cuisinier = new Cuisinier(nom, prenom, TestUtils.StationTest, telephone, email);

            // Assert
            if (cuisinier.Nom != nom) throw new Exception("Le nom n'est pas correct");
            if (cuisinier.Prenom != prenom) throw new Exception("Le prénom n'est pas correct");
            if (cuisinier.Station != TestUtils.StationTest) throw new Exception("La station n'est pas correcte");
            if (cuisinier.Telephone != telephone) throw new Exception("Le téléphone n'est pas correct");
            if (cuisinier.Email != email) throw new Exception("L'email n'est pas correct");
        }

        public void Test_ToString_Cuisinier()
        {
            // Arrange
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");

            // Act
            var resultat = cuisinier.ToString();

            // Assert
            if (resultat != "Cuisinier : Sophie Martin") throw new Exception("La représentation textuelle n'est pas correcte");
        }
    }
} 