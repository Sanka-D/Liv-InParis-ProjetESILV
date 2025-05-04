using System;
using System.Collections.Generic;

namespace LivinParis.Models.Trajets
{
    /// <summary>
    /// Represents a metro station in the network.
    /// </summary>
    public class Station
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Ligne { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Station Precedente { get; set; }
        public double Distance { get; set; }
        public List<Connexion> Connexions { get; set; }

        /// <summary>
        /// Initializes a new instance of the Station class.
        /// </summary>
        public Station(int id, string nom, string adresse, double latitude, double longitude)
        {
            Id = id;
            Nom = nom;
            Adresse = adresse;
            Latitude = latitude;
            Longitude = longitude;
            Connexions = new List<Connexion>();
        }

        public Station(int id, string nom, string ligne, double latitude, double longitude, bool isLigne)
            : this(id, nom, "", latitude, longitude)
        {
            Ligne = ligne;
        }

        /// <summary>
        /// Calculates the distance between this station and another using the Haversine formula.
        /// </summary>
        public double CalculerDistance(Station autre)
        {
            const double R = 6371; /// Earth's radius in km
            var lat1 = Latitude * Math.PI / 180;
            var lat2 = autre.Latitude * Math.PI / 180;
            var deltaLat = (autre.Latitude - Latitude) * Math.PI / 180;
            var deltaLon = (autre.Longitude - Longitude) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public override string ToString()
        {
            return $"{Nom} ({(string.IsNullOrEmpty(Adresse) ? Ligne : Adresse)})";
        }
    }
} 