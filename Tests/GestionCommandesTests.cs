using System;
using LivinParis.Models.Client;
using LivinParis.Models.Commande;
using LivinParis.Models.Cuisinier;

namespace LivinParis.Tests
{
    public class GestionCommandesTests
    {
        public void Test_Creation_Et_Modification_Commande()
        {
            // Arrange
            var client = new Client("Dupont", "Jean", "123 rue de Paris", "0123456789", "jean@email.com");
            GestionClients.AjouterClient(client);

            // Act - Création commande
            var commande = GestionCommandes.CreerCommande(client.Identifiant, client.Adresse);
            if (commande.Statut != StatutCommande.EnAttente)
                throw new Exception("La commande devrait être en attente");

            // Act - Ajout lignes
            commande.AjouterLigne("Coq au vin", 2, 15.0m);
            commande.AjouterLigne("Ratatouille", 1, 12.0m);

            // Assert - Vérification du montant
            decimal montantAttendu = (2 * 15.0m) + (1 * 12.0m);
            if (commande.MontantTotal != montantAttendu)
                throw new Exception("Le montant total n'est pas correct");

            // Act - Modification ligne
            commande.ModifierLigne(0, 3); // Modifier la quantité de Coq au vin à 3

            // Assert - Vérification du nouveau montant
            montantAttendu = (3 * 15.0m) + (1 * 12.0m);
            if (commande.MontantTotal != montantAttendu)
                throw new Exception("Le montant total après modification n'est pas correct");
        }

        public void Test_Cycle_Vie_Commande()
        {
            // Arrange
            var client = new Client("Martin", "Sophie", "456 rue de Lyon", "9876543210", "sophie@email.com");
            GestionClients.AjouterClient(client);
            var cuisinier = new Cuisinier("Petit", "Pierre", "789 rue de Marseille", "0123456789", "pierre@cuisine.com");
            GestionCuisiniers.AjouterCuisinier(cuisinier);

            // Act - Création et assignation
            var commande = GestionCommandes.CreerCommande(client.Identifiant, client.Adresse);
            GestionCommandes.AssignerCuisinier(commande.Identifiant, cuisinier.Identifiant);

            // Assert - Vérification du statut
            if (commande.Statut != StatutCommande.EnPreparation)
                throw new Exception("La commande devrait être en préparation");

            // Act - Simulation des étapes
            GestionCommandes.DemarrerPreparation(commande.Identifiant);
            GestionCommandes.DemarrerLivraison(commande.Identifiant);
            if (commande.Statut != StatutCommande.EnLivraison)
                throw new Exception("La commande devrait être en livraison");

            // Act - Terminer la commande
            GestionCommandes.TerminerCommande(commande.Identifiant);
            if (commande.Statut != StatutCommande.Livree)
                throw new Exception("La commande devrait être livrée");
        }
    }
} 