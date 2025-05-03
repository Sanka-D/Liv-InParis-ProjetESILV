namespace LivinParis.Models.Trajets
{
    /// <summary>
    /// Represents a connection between two stations in the metro network.
    /// </summary>
    public class Connexion
    {
        /// <summary>
        /// Gets or sets the ID of the departure station.
        /// </summary>
        public int StationDepartId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the arrival station.
        /// </summary>
        public int StationArriveeId { get; set; }

        public Station Station { get; set; }
        public double Distance { get; set; }

        public Connexion(Station station, double distance)
        {
            Station = station;
            Distance = distance;
        }
    }
} 