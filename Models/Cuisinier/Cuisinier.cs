using System;
using LivinParis.Models.Client;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Cuisinier
{
    public class Cuisinier : Utilisateur
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public Station Station { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public Cuisinier(string nom, string prenom, Station station, string telephone, string email)
            : base()
        {
            Nom = nom;
            Prenom = prenom;
            Station = station;
            Telephone = telephone;
            Email = email;
        }

        public override string ToString()
        {
            return $"Cuisinier : {Prenom} {Nom}";
        }
    }
} 