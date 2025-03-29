using System;
using LivinParis.Models.Trajets;

namespace LivinParis.Tests
{
    public class ReseauMetroTests
    {
        public void Test_Reseau_Metro()
        {
            // Arrange
            var reseau = new ReseauMetro();

            // Créer quelques stations
            var station1 = new Station(1, "Gare du Nord", "18 Boulevard de Denain, 75010 Paris", 48.8800, 2.3550);
            var station2 = new Station(2, "Gare de l'Est", "Place du 11 Novembre 1918, 75010 Paris", 48.8760, 2.3590);
            var station3 = new Station(3, "Gare Saint-Lazare", "13 Rue d'Amsterdam, 75008 Paris", 48.8760, 2.3240);
            var station4 = new Station(4, "Gare Montparnasse", "17 Boulevard de Vaugirard, 75015 Paris", 48.8400, 2.3200);

            reseau.AjouterStation(station1);
            reseau.AjouterStation(station2);
            reseau.AjouterStation(station3);
            reseau.AjouterStation(station4);

            // Ajouter des connexions
            reseau.AjouterConnexion(1, 2); // Gare du Nord - Gare de l'Est
            reseau.AjouterConnexion(2, 3); // Gare de l'Est - Gare Saint-Lazare
            reseau.AjouterConnexion(3, 4); // Gare Saint-Lazare - Gare Montparnasse

            // Act & Assert
            try
            {
                // Test de recherche de chemin
                reseau.ComparerAlgorithmes(1, 4); // De Gare du Nord à Gare Montparnasse
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la comparaison des algorithmes : {ex.Message}");
            }

            // Test d'obtention d'une station
            var station = reseau.ObtenirStation(1);
            if (station == null || station.Id != 1 || station.Nom != "Gare du Nord")
                throw new Exception("La station n'a pas été correctement récupérée");

            // Test d'ajout de connexion avec stations inexistantes
            try
            {
                reseau.AjouterConnexion(1, 999);
                throw new Exception("L'ajout d'une connexion avec une station inexistante aurait dû échouer");
            }
            catch (ArgumentException) { /* OK */ }
        }
    }
} 