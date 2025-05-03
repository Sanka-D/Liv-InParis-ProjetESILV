using System;

namespace LivinParis.Models.Trajets
{
    /// <summary>
    /// Represents a metro line connecting two stations.
    /// </summary>
    public class Ligne
    {
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
        /// <param name="stationDepart">The departure station.</param>
        /// <param name="stationArrivee">The arrival station.</param>
        /// <param name="duree">The duration of travel in minutes.</param>
        public Ligne(Station stationDepart, Station stationArrivee, double duree)
        {
            StationDepart = stationDepart;
            StationArrivee = stationArrivee;
            Duree = duree;
        }

        public override string ToString()
        {
            return $"Ligne {Nom} : {StationDepart} → {StationArrivee} ({Duree} min)";
        }
    }
} 