using System;
using LivinParis.Models.Trajets;

namespace LivinParis.Models.Client
{
    public class Client : Utilisateur
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public Station Station { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool EstEntreprise { get; set; }
        public string NomEntreprise { get; set; }
        public string ReferentEntreprise { get; set; }
        public decimal MontantAchats { get; set; }

        public Client(string nom, string prenom, Station station, string telephone, string email, 
                    bool estEntreprise = false, string nomEntreprise = null, string referentEntreprise = null)
            : base()
        {
            Nom = nom;
            Prenom = prenom;
            Station = station;
            Telephone = telephone;
            Email = email;
            EstEntreprise = estEntreprise;
            NomEntreprise = nomEntreprise;
            ReferentEntreprise = referentEntreprise;
            MontantAchats = 0;
        }

        public void AjouterAchat(decimal montant)
        {
            if (montant < 0)
                throw new ArgumentException("Le montant ne peut pas être négatif");
                
            MontantAchats += montant;
        }

        public override string ToString()
        {
            if (EstEntreprise)
            {
                return $"Entreprise : {NomEntreprise} - Référent : {ReferentEntreprise}";
            }
            return $"Client : {Prenom} {Nom}";
        }
    }
} 