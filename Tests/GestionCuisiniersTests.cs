using System;
using System.Linq;
using LivinParis.Models.Client;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Tests
{
    public class GestionCuisiniersTests
    {
        public void Test_Gestion_Cuisinier()
        {
            // Arrange
            var cuisinier = new Cuisinier("Petit", "Pierre", "789 rue de Marseille", "0123456789", "pierre@cuisine.com");

            // Test Ajout
            GestionCuisiniers.AjouterCuisinier(cuisinier);
            if (GestionCuisiniers.ObtenirPlatDuJour(cuisinier.Identifiant) != "Aucun plat du jour défini")
                throw new Exception("Le plat du jour devrait être vide");

            // Test Modification
            var nouveauCuisinier = new Cuisinier("Petit", "Pierre", "123 rue de Paris", "9876543210", "pierre.nouveau@cuisine.com");
            GestionCuisiniers.ModifierCuisinier(cuisinier.Identifiant, nouveauCuisinier);
            if (GestionCuisiniers.ObtenirPlatDuJour(cuisinier.Identifiant) != "Aucun plat du jour défini")
                throw new Exception("Le plat du jour devrait être vide après modification");

            // Test Suppression
            GestionCuisiniers.SupprimerCuisinier(cuisinier.Identifiant);
            try
            {
                GestionCuisiniers.ObtenirPlatDuJour(cuisinier.Identifiant);
                throw new Exception("Le cuisinier devrait avoir été supprimé");
            }
            catch (Exception) { /* OK */ }
        }

        public void Test_Plats_Et_Clients()
        {
            // Arrange
            var cuisinier = new Cuisinier("Petit", "Pierre", "789 rue de Marseille", "0123456789", "pierre@cuisine.com");
            var client = new Client("Dupont", "Jean", "123 rue de Paris", "0123456789", "jean@email.com");

            // Act
            GestionCuisiniers.AjouterCuisinier(cuisinier);
            GestionCuisiniers.AjouterClientServi(cuisinier.Identifiant, client);
            GestionCuisiniers.AjouterPlatRealise(cuisinier.Identifiant, "Coq au vin");
            GestionCuisiniers.AjouterPlatRealise(cuisinier.Identifiant, "Coq au vin");
            GestionCuisiniers.AjouterPlatRealise(cuisinier.Identifiant, "Ratatouille");
            GestionCuisiniers.DefinirPlatDuJour(cuisinier.Identifiant, "Coq au vin");

            // Assert
            var plats = GestionCuisiniers.ObtenirPlatsParFrequence(cuisinier.Identifiant);
            if (plats.Count != 2) throw new Exception("Il devrait y avoir 2 plats différents");
            if (plats[0].Plat != "Coq au vin" || plats[0].Frequence != 2) 
                throw new Exception("Le Coq au vin devrait apparaître 2 fois");
            if (plats[1].Plat != "Ratatouille" || plats[1].Frequence != 1) 
                throw new Exception("La Ratatouille devrait apparaître 1 fois");
            if (GestionCuisiniers.ObtenirPlatDuJour(cuisinier.Identifiant) != "Coq au vin")
                throw new Exception("Le plat du jour devrait être Coq au vin");
        }
    }
} 