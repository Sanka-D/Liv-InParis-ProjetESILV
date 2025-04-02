using System;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Tests
{
    public class GestionCuisiniersTests
    {
        public void Test_Ajouter_Modifier_Supprimer_Cuisinier()
        {
            // Arrange
            var cuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "9876543210", "sophie@cuisine.com");

            // Act - Ajout
            GestionCuisiniers.AjouterCuisinier(cuisinier);
            var cuisiniers = GestionCuisiniers.ObtenirCuisiniers();
            if (!cuisiniers.Contains(cuisinier)) throw new Exception("Le cuisinier n'a pas été ajouté");

            // Act - Modification
            var nouveauCuisinier = new Cuisinier("Martin", "Sophie", TestUtils.StationTest, "1122334455", "sophie.martin@cuisine.com");
            GestionCuisiniers.ModifierCuisinier(cuisinier.Identifiant, nouveauCuisinier);
            var cuisinierModifie = GestionCuisiniers.ObtenirCuisinier(cuisinier.Identifiant);
            if (cuisinierModifie.Telephone != "1122334455") throw new Exception("Le téléphone n'a pas été modifié");
            if (cuisinierModifie.Email != "sophie.martin@cuisine.com") throw new Exception("L'email n'a pas été modifié");
            if (cuisinierModifie.Station != TestUtils.StationTest) throw new Exception("La station n'a pas été modifiée");

            // Act - Suppression
            GestionCuisiniers.SupprimerCuisinier(cuisinier.Identifiant);
            cuisiniers = GestionCuisiniers.ObtenirCuisiniers();
            if (cuisiniers.Contains(cuisinier)) throw new Exception("Le cuisinier n'a pas été supprimé");
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