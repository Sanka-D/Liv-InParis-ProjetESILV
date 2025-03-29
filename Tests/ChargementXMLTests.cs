using System;
using LivinParis.Models.Trajets;

namespace LivinParis.Tests
{
    public class ChargementXMLTests
    {
        public void Test_Chargement_XML()
        {
            // Arrange
            string cheminFichier = "./Data/metro.xml";

            // Act
            var reseau = ChargementXML.ChargerReseau(cheminFichier);

            // Assert
            if (reseau == null)
                throw new Exception("Le réseau n'a pas été chargé");

            // Vérifier que nous avons des stations
            var station = reseau.ObtenirStation(2212); // Château de Vincennes
            if (station == null)
                throw new Exception("La première station n'a pas été chargée");

            // Afficher quelques statistiques
            Console.WriteLine($"\nRéseau chargé avec succès :");
            Console.WriteLine($"Nombre de stations : {reseau.Stations.Count}");
            
            // Tester un trajet
            try
            {
                reseau.ComparerAlgorithmes(2212, 2080); // De Château de Vincennes à Bérault
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la comparaison des algorithmes : {ex.Message}");
            }
        }
    }
} 