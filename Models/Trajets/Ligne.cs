using System;
using System.Collections.Generic;

namespace LivinParis.Models.Trajets
{
    /// <summary>
    /// Represents a metro line connecting two stations.
    /// </summary>
    public class Ligne
    {
        /// <summary>
        /// Gets or sets the ID of the line.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the line.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        public string Couleur { get; set; }

        /// <summary>
        /// Gets or sets the list of stations on the line.
        /// </summary>
        public List<Station> Stations { get; set; }

        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
        public double Longueur { get; set; }

        /// <summary>
        /// Gets or sets the departure station.
        /// </summary>
        public Station StationDepart { get; set; }

        /// <summary>
        /// Gets or sets the arrival station.
        /// </summary>
        public Station StationArrivee { get; set; }

        /// <summary>
        /// Gets or sets the duration of travel between stations in minutes.
        /// </summary>
        public double Duree { get; set; }

        /// <summary>
        /// Initializes a new instance of the Ligne class.
        /// </summary>
        public Ligne()
        {
            Stations = new List<Station>();
        }

        /// <summary>
        /// Initializes a new instance of the Ligne class.
        /// </summary>
        /// <param name="id">The ID of the line.</param>
        /// <param name="nom">The name of the line.</param>
        /// <param name="stationDepart">The departure station.</param>
        /// <param name="stationArrivee">The arrival station.</param>
        /// <param name="duree">The duration of travel in minutes.</param>
        public Ligne(int id, string nom, Station stationDepart, Station stationArrivee, double duree)
        {
            Id = id;
            Nom = nom;
            StationDepart = stationDepart ?? throw new ArgumentNullException(nameof(stationDepart));
            StationArrivee = stationArrivee ?? throw new ArgumentNullException(nameof(stationArrivee));
            Duree = duree;
            Stations = new List<Station> { stationDepart, stationArrivee };
            Longueur = stationDepart.CalculerDistance(stationArrivee);
        }

        /// <summary>
        /// Adds a station to the line.
        /// </summary>
        /// <param name="station">The station to add.</param>
        public void AjouterStation(Station station)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));

            if (!Stations.Contains(station))
            {
                Stations.Add(station);
                if (Stations.Count > 1)
                {
                    // Update length when adding new stations
                    var previousStation = Stations[Stations.Count - 2];
                    Longueur += previousStation.CalculerDistance(station);
                }
            }
        }

        public override string ToString()
        {
            return $"{Nom} ({StationDepart.Nom} → {StationArrivee.Nom})";
        }
    }
} 