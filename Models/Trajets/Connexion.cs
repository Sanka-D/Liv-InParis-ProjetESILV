namespace LivinParis.Models.Trajets
{
    public class Connexion
    {
        public Station Station { get; set; }
        public double Distance { get; set; }

        public Connexion(Station station, double distance)
        {
            Station = station;
            Distance = distance;
        }
    }
} 