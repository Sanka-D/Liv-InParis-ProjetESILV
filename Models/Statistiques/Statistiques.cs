using System;
using System.Collections.Generic;

namespace LivinParis.Models.Statistiques
{
    /// <summary>
    /// Represents statistics for the system.
    /// </summary>
    public class Statistiques
    {
        /// <summary>
        /// Gets or sets the total number of orders.
        /// </summary>
        public int NombreCommandes { get; set; }

        /// <summary>
        /// Gets or sets the total revenue.
        /// </summary>
        public decimal ChiffreAffaires { get; set; }

        /// <summary>
        /// Gets or sets the average order value.
        /// </summary>
        public decimal PanierMoyen { get; set; }

        /// <summary>
        /// Gets or sets the number of active clients.
        /// </summary>
        public int ClientsActifs { get; set; }

        /// <summary>
        /// Gets or sets the number of active cooks.
        /// </summary>
        public int CuisiniersActifs { get; set; }

        /// <summary>
        /// Gets or sets the list of most popular dishes.
        /// </summary>
        public List<string> PlatsPopulaires { get; set; }

        /// <summary>
        /// Initializes a new instance of the Statistiques class.
        /// </summary>
        public Statistiques()
        {
            PlatsPopulaires = new List<string>();
        }
    }
} 