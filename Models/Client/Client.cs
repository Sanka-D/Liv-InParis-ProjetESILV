using System;

namespace LivinParis.Models.Client
{
    public class Client : Utilisateur
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool EstEntreprise { get; set; }
        public string NomEntreprise { get; set; }
        public string ReferentEntreprise { get; set; }
        public decimal MontantAchats { get; set; }

        public Client(string nom, string prenom, string adresse, string telephone, string email, 
                    bool estEntreprise = false, string nomEntreprise = null, string referentEntreprise = null)
            : base()
        {
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
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