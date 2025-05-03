using Microsoft.VisualStudio.TestTools.UnitTesting;
using LivinParis.Models;
using System.Collections.Generic;

namespace LivinParis.Tests
{
    /// <summary>
    /// Test class for the ReseauMetro class.
    /// </summary>
    [TestClass]
    public class ReseauMetroTests
    {
        private ReseauMetro reseau;
        private Station station1;
        private Station station2;
        private Station station3;
        private Station station4;

        /// <summary>
        /// Initializes test data before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            station1 = new Station(1, "Gare du Nord", "Ligne 4", 48.8800, 2.3550);
            station2 = new Station(2, "Châtelet", "Ligne 4", 48.8600, 2.3470);
            station3 = new Station(3, "Nation", "Ligne 1", 48.8483, 2.3962);
            station4 = new Station(4, "Bastille", "Ligne 1", 48.8531, 2.3692);

            reseau = new ReseauMetro();
            reseau.AjouterStation(station1);
            reseau.AjouterStation(station2);
            reseau.AjouterStation(station3);
            reseau.AjouterStation(station4);
        }

        /// <summary>
        /// Tests the station search functionality by name.
        /// </summary>
        [TestMethod]
        public void TestRechercherStationParNom()
        {
            var station = reseau.RechercherStationParNom("Châtelet");
            Assert.AreEqual(station2, station);
        }

        /// <summary>
        /// Tests the station search functionality with a non-existent station.
        /// </summary>
        [TestMethod]
        public void TestRechercherStationInexistante()
        {
            var station = reseau.RechercherStationParNom("Station Inexistante");
            Assert.IsNull(station);
        }
    }
} 