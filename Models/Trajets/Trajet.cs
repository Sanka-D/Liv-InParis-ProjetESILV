using System;

namespace LivinParis.Models.Trajets
{
    public class Trajet
    {
        public int Id { get; set; }
        public int CommandeId { get; set; }
        public string AdresseDepart { get; set; }
        public string AdresseArrivee { get; set; }
        public DateTime HeureDepart { get; set; }
        public DateTime? HeureArrivee { get; set; }
        public string Statut { get; set; }
        public string LivreurId { get; set; }

        public Trajet(int id, int commandeId, string adresseDepart, string adresseArrivee, string livreurId)
        {
            Id = id;
            CommandeId = commandeId;
            AdresseDepart = adresseDepart;
            AdresseArrivee = adresseArrivee;
            HeureDepart = DateTime.Now;
            Statut = "En cours";
            LivreurId = livreurId;
        }
    }
} 