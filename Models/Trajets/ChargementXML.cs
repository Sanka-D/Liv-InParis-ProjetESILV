using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;

namespace LivinParis.Models.Trajets
{
    public class ChargementXML
    {
        public static ReseauMetro ChargerReseau(string cheminFichier)
        {
            var reseau = new ReseauMetro();
            var doc = XDocument.Load(cheminFichier);

            // Charger toutes les stations
            var stationsElement = doc.Root.Element("stations");
            if (stationsElement == null)
                throw new Exception("L'élément 'stations' n'a pas été trouvé dans le fichier XML");

            foreach (var stationElement in stationsElement.Elements("station"))
            {
                var idStr = stationElement.Attribute("id").Value;
                var id = int.Parse(idStr.Substring(2)); // Enlever le préfixe "id"
                var station = new Station(
                    id,
                    stationElement.Element("nom").Value,
                    stationElement.Element("adresse").Value,
                    double.Parse(stationElement.Element("latitude").Value),
                    double.Parse(stationElement.Element("longitude").Value)
                );
                reseau.AjouterStation(station);
            }

            // Charger les connexions à partir des trajets
            var servicesElement = doc.Root.Element("services");
            if (servicesElement != null)
            {
                foreach (var serviceElement in servicesElement.Elements("service"))
                {
                    foreach (var trajetElement in serviceElement.Elements("trajet"))
                    {
                        var stations = trajetElement.Elements("station")
                            .Select(s => int.Parse(s.Attribute("ref-id").Value.Substring(2)))
                            .ToList();

                        // Créer les connexions entre les stations consécutives
                        for (int i = 0; i < stations.Count - 1; i++)
                        {
                            try
                            {
                                reseau.AjouterConnexion(stations[i], stations[i + 1]);
                            }
                            catch (ArgumentException)
                            {
                                // Ignorer les connexions invalides
                                continue;
                            }
                        }
                    }
                }
            }

            return reseau;
        }

        public static void SauvegarderReseau(ReseauMetro reseau, string cheminFichier)
        {
            var doc = new XDocument(
                new XElement("reseau",
                    new XElement("stations",
                        // Sauvegarder les stations
                        reseau.Stations.Values.Select(s =>
                            new XElement("station",
                                new XAttribute("id", s.Id),
                                new XElement("nom", s.Nom),
                                new XElement("adresse", s.Adresse),
                                new XElement("latitude", s.Latitude),
                                new XElement("longitude", s.Longitude)
                            )
                        )
                    ),
                    new XElement("connexions",
                        // Sauvegarder les connexions
                        reseau.Graphe.Noeuds.Values.SelectMany(n =>
                            n.Voisins.Select(v =>
                                new XElement("connexion",
                                    new XElement("station1", n.Valeur.Id),
                                    new XElement("station2", v.Key.Valeur.Id)
                                )
                            )
                        )
                    )
                )
            );

            doc.Save(cheminFichier);
        }
    }
} 