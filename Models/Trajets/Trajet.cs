using System;
using System.Collections.Generic;

namespace LivinParis.Models.Trajets
{
    /// <summary>
    /// Represents a journey in the metro network.
    /// </summary>
    public class Trajet
    {
        /// <summary>
        /// Gets or sets the list of stations in the journey.
        /// </summary>
        public List<Station> Stations { get; set; }

        /// <summary>
        /// Gets or sets the total distance of the journey in kilometers.
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Gets or sets the estimated duration of the journey in minutes.
        /// </summary>
        public double Duree { get; set; }

        /// <summary>
        /// Initializes a new instance of the Trajet class.
        /// </summary>
        public Trajet()
        {
            Stations = new List<Station>();
        }

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