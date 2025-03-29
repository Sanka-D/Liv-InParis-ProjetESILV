using System;
using LivinParis.Models.Client;
using LivinParis.Models.Commande;
using LivinParis.Models.Cuisinier;
using LivinParis.Models.Statistiques;

namespace LivinParis.Tests
{
    public class GestionStatistiquesTests
    {
        public void Test_Statistiques_Commandes()
        {
            // Arrange
            GestionCommandes.ReinitialiserCommandes();
            var client = new Client("Dupont", "Jean", "123 rue de Paris", "0123456789", "jean@email.com");
            var cuisinier = new Cuisinier("Petit", "Pierre", "789 rue de Marseille", "0123456789", "pierre@cuisine.com");
            
            GestionClients.AjouterClient(client);
            GestionCuisiniers.AjouterCuisinier(cuisinier);

            // Créer quelques commandes
            var commande1 = GestionCommandes.CreerCommande(client.Identifiant, client.Adresse);
            commande1.AjouterLigne("Coq au vin", 2, 15.0m);
            commande1.AjouterLigne("Ratatouille", 1, 12.0m);
            GestionCommandes.AssignerCuisinier(commande1.Identifiant, cuisinier.Identifiant);
            GestionCommandes.TerminerCommande(commande1.Identifiant);

            var commande2 = GestionCommandes.CreerCommande(client.Identifiant, client.Adresse);
            commande2.AjouterLigne("Pizza Margherita", 1, 14.0m);
            commande2.AjouterLigne("Pasta Carbonara", 2, 13.0m);
            GestionCommandes.AssignerCuisinier(commande2.Identifiant, cuisinier.Identifiant);
            GestionCommandes.TerminerCommande(commande2.Identifiant);

            // Test des livraisons par cuisinier
            var livraisonsParCuisinier = GestionStatistiques.ObtenirLivraisonsParCuisinier();
            if (!livraisonsParCuisinier.ContainsKey(cuisinier.Identifiant))
                throw new Exception("Le cuisinier devrait avoir des livraisons");
            if (livraisonsParCuisinier[cuisinier.Identifiant] != 2)
                throw new Exception("Le cuisinier devrait avoir 2 livraisons");

            // Test des moyennes
            decimal moyennePrixCommandes = GestionStatistiques.CalculerMoyennePrixCommandes();
            decimal moyenneAttendue = ((2 * 15.0m + 1 * 12.0m) + (1 * 14.0m + 2 * 13.0m)) / 2;
            if (moyennePrixCommandes != moyenneAttendue)
                throw new Exception("La moyenne des prix des commandes n'est pas correcte");

            // Test des commandes par nationalité
            var commandesParNationalite = GestionStatistiques.ObtenirCommandesParNationalitePlats();
            if (!commandesParNationalite.ContainsKey("Française") || commandesParNationalite["Française"] != 3)
                throw new Exception("Il devrait y avoir 3 plats français");
            if (!commandesParNationalite.ContainsKey("Italienne") || commandesParNationalite["Italienne"] != 3)
                throw new Exception("Il devrait y avoir 3 plats italiens");

            // Test des commandes sur période
            var debut = DateTime.Now.AddDays(-1);
            var fin = DateTime.Now.AddDays(1);
            var commandesSurPeriode = GestionStatistiques.ObtenirCommandesSurPeriode(debut, fin);
            if (commandesSurPeriode.Count != 2)
                throw new Exception("Il devrait y avoir 2 commandes sur la période");

            // Afficher les statistiques pour vérification visuelle
            GestionStatistiques.AfficherStatistiques(debut, fin);
        }
    }
} 