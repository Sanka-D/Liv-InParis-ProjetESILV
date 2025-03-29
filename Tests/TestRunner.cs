using System;

namespace LivinParis.Tests
{
    public class TestRunner
    {
        public static void ExecuterTousLesTests()
        {
            Console.WriteLine("Démarrage des tests...\n");

            // Tests Client
            var clientTests = new ClientTests();
            ExecuterTest("Test_Creation_Client_Particulier", clientTests.Test_Creation_Client_Particulier);
            ExecuterTest("Test_Creation_Client_Entreprise", clientTests.Test_Creation_Client_Entreprise);

            // Tests Cuisinier
            var cuisinierTests = new CuisinierTests();
            ExecuterTest("Test_Creation_Cuisinier", cuisinierTests.Test_Creation_Cuisinier);
            ExecuterTest("Test_ToString_Cuisinier", cuisinierTests.Test_ToString_Cuisinier);

            // Tests Authentification
            var authentificationTests = new AuthentificationTests();
            ExecuterTest("Test_Enregistrement_Et_Authentification", authentificationTests.Test_Enregistrement_Et_Authentification);
            ExecuterTest("Test_Authentification_Echec", authentificationTests.Test_Authentification_Echec);

            // Tests GestionClients
            var gestionClientsTests = new GestionClientsTests();
            ExecuterTest("Test_Ajouter_Client", gestionClientsTests.Test_Ajouter_Client);
            ExecuterTest("Test_Modifier_Client", gestionClientsTests.Test_Modifier_Client);
            ExecuterTest("Test_Supprimer_Client", gestionClientsTests.Test_Supprimer_Client);
            ExecuterTest("Test_Tri_Clients", gestionClientsTests.Test_Tri_Clients);

            // Tests GestionCuisiniers
            var gestionCuisiniersTests = new GestionCuisiniersTests();
            ExecuterTest("Test_Gestion_Cuisinier", gestionCuisiniersTests.Test_Gestion_Cuisinier);
            ExecuterTest("Test_Plats_Et_Clients", gestionCuisiniersTests.Test_Plats_Et_Clients);

            // Tests GestionCommandes
            var gestionCommandesTests = new GestionCommandesTests();
            ExecuterTest("Test_Creation_Et_Modification_Commande", gestionCommandesTests.Test_Creation_Et_Modification_Commande);
            ExecuterTest("Test_Cycle_Vie_Commande", gestionCommandesTests.Test_Cycle_Vie_Commande);

            // Tests GestionStatistiques
            var gestionStatistiquesTests = new GestionStatistiquesTests();
            ExecuterTest("Test_Statistiques_Commandes", gestionStatistiquesTests.Test_Statistiques_Commandes);

            // Tests ReseauMetro
            var reseauMetroTests = new ReseauMetroTests();
            ExecuterTest("Test_Reseau_Metro", reseauMetroTests.Test_Reseau_Metro);

            // Tests ChargementXML
            var chargementXMLTests = new ChargementXMLTests();
            ExecuterTest("Test_Chargement_XML", chargementXMLTests.Test_Chargement_XML);

            Console.WriteLine("\nTous les tests sont terminés.");
        }

        private static void ExecuterTest(string nomTest, Action test)
        {
            try
            {
                test();
                Console.WriteLine($"✓ {nomTest} : Succès");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ {nomTest} : Échec");
                Console.WriteLine($"  Erreur : {ex.Message}\n");
            }
        }
    }
} 