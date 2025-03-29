using System;

namespace LivinParis.Models.Client
{
    public abstract class Utilisateur
    {
        public string Identifiant { get; private set; }
        private string _motDePasse;

        protected Utilisateur()
        {
            Identifiant = GenererIdentifiantUnique();
        }

        public bool VerifierMotDePasse(string motDePasse)
        {
            return _motDePasse == motDePasse;
        }

        public void DefinirMotDePasse(string motDePasse)
        {
            _motDePasse = motDePasse;
        }

        private string GenererIdentifiantUnique()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
} 