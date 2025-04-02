using System;

namespace LivinParis.Models.Trajets
{
    public class Ligne
    {
        public int Id { get; private set; }
        public string Nom { get; private set; }
        public Station StationDepart { get; private set; }
        public Station StationArrivee { get; private set; }
        public double Duree { get; private set; }

        public Ligne(int id, string nom, Station stationDepart, Station stationArrivee, double duree)
        {
            Id = id;
            Nom = nom;
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