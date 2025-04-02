using System;
using System.Collections.Generic;
using System.Linq;

namespace LivinParis.Tests
{
    public class TestRunner
    {
        public void ExecuterTousLesTests()
        {
            var tests = new List<(string Nom, Action Test)>
            {
                ("Authentification - Enregistrement et authentification", () => new AuthentificationTests().Test_Enregistrement_Et_Authentification()),
                ("Authentification - Échec d'authentification", () => new AuthentificationTests().Test_Authentification_Echec()),
                ("Client - Création client particulier", () => new ClientTests().Test_Creation_Client_Particulier()),
                ("Client - Création client entreprise", () => new ClientTests().Test_Creation_Client_Entreprise()),
                ("Cuisinier - Création", () => new CuisinierTests().Test_Creation_Cuisinier()),
                ("Cuisinier - ToString", () => new CuisinierTests().Test_ToString_Cuisinier()),
                ("GestionClients - Ajout", () => new GestionClientsTests().Test_Ajouter_Client()),
                ("GestionClients - Modification", () => new GestionClientsTests().Test_Modifier_Client()),
                ("GestionClients - Suppression", () => new GestionClientsTests().Test_Supprimer_Client()),
                ("GestionClients - Tri", () => new GestionClientsTests().Test_Tri_Clients()),
                ("GestionCuisiniers - Ajout/Modification/Suppression", () => new GestionCuisiniersTests().Test_Ajouter_Modifier_Supprimer_Cuisinier()),
                ("GestionCuisiniers - Gestion des plats", () => new GestionCuisiniersTests().Test_Gestion_Plats()),
                ("GestionCuisiniers - Gestion des clients servis", () => new GestionCuisiniersTests().Test_Gestion_Clients_Servis()),
                ("GestionCommandes - Création", () => new GestionCommandesTests().Test_Creation_Commande()),
                ("GestionCommandes - Modification", () => new GestionCommandesTests().Test_Modification_Commande()),
                ("GestionCommandes - Suppression", () => new GestionCommandesTests().Test_Suppression_Commande()),
                ("GestionCommandes - Simulation", () => new GestionCommandesTests().Test_Simulation_Commande()),
                ("GestionStatistiques - Statistiques globales", () => new GestionStatistiquesTests().Test_Statistiques_Globales()),
                ("GestionStatistiques - Statistiques par client", () => new GestionStatistiquesTests().Test_Statistiques_Par_Client()),
                ("GestionStatistiques - Statistiques par cuisinier", () => new GestionStatistiquesTests().Test_Statistiques_Par_Cuisinier())
            };

            foreach (var test in tests)
            {
                try
                {
                    test.Test();
                    Console.WriteLine($"✅ {test.Nom} : Succès");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ {test.Nom} : Échec - {ex.Message}");
                }
            }
        }
    }
} 