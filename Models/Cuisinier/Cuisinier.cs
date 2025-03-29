using System;
using LivinParis.Models.Client;

namespace LivinParis.Models.Cuisinier
{
    public class Cuisinier : Utilisateur
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public Cuisinier(string nom, string prenom, string adresse, string telephone, string email)
            : base()
        {
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
        }

        public override string ToString()
        {
            return $"Cuisinier : {Prenom} {Nom}";
        }
    }
} 