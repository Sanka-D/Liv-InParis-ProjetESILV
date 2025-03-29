using System;

namespace LivinParis.Models.Statistiques
{
    public class Statistiques
    {
        public int NombreTotalCommandes { get; set; }
        public decimal ChiffreAffairesTotal { get; set; }
        public int NombreClientsUniques { get; set; }
        public double NoteMoyenne { get; set; }
        public DateTime PeriodeDebut { get; set; }
        public DateTime PeriodeFin { get; set; }

        public Statistiques(DateTime periodeDebut, DateTime periodeFin)
        {
            PeriodeDebut = periodeDebut;
            PeriodeFin = periodeFin;
            NombreTotalCommandes = 0;
            ChiffreAffairesTotal = 0;
            NombreClientsUniques = 0;
            NoteMoyenne = 0;
        }
    }
} 