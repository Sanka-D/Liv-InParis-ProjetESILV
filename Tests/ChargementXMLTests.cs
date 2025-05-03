using Microsoft.VisualStudio.TestTools.UnitTesting;
using LivinParis.Models;
using System.IO;
using System.Xml.Linq;

namespace LivinParis.Tests
{
    /// <summary>
    /// Test class for XML loading and saving functionality.
    /// </summary>
    [TestClass]
    public class ChargementXMLTests
    {
        private string testFilePath;
        private ReseauMetro reseau;

        /// <summary>
        /// Initializes test data before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            testFilePath = "test_metro.xml";
            reseau = new ReseauMetro();
            var station1 = new Station(1, "Gare du Nord", "Ligne 4", 48.8800, 2.3550);
            var station2 = new Station(2, "Châtelet", "Ligne 4", 48.8600, 2.3470);
            reseau.AjouterStation(station1);
            reseau.AjouterStation(station2);
            reseau.AjouterConnexion(1, 2);
        }

        /// <summary>
        /// Tests saving the metro network to XML.
        /// </summary>
        [TestMethod]
        public void TestSauvegarderReseau()
        {
            ChargementXML.SauvegarderReseau(reseau, testFilePath);
            Assert.IsTrue(File.Exists(testFilePath));
        }

        /// <summary>
        /// Tests loading the metro network from XML.
        /// </summary>
        [TestMethod]
        public void TestChargerReseau()
        {
            ChargementXML.SauvegarderReseau(reseau, testFilePath);
            var reseauCharge = ChargementXML.ChargerReseau(testFilePath);
            Assert.IsNotNull(reseauCharge);
            Assert.AreEqual(2, reseauCharge.Stations.Count);
        }

        /// <summary>
        /// Cleans up test files after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }
    }
} 