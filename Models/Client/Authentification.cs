using System;
using System.Collections.Generic;

namespace LivinParis.Models.Client
{
    public class Authentification
    {
        private static Dictionary<string, Utilisateur> _utilisateurs = new Dictionary<string, Utilisateur>();

        public static void EnregistrerUtilisateur(Utilisateur utilisateur, string motDePasse)
        {
            utilisateur.DefinirMotDePasse(motDePasse);
            _utilisateurs.Add(utilisateur.Identifiant, utilisateur);
        }

        public static Utilisateur Authentifier(string identifiant, string motDePasse)
        {
            if (_utilisateurs.TryGetValue(identifiant, out Utilisateur utilisateur))
            {
                if (utilisateur.VerifierMotDePasse(motDePasse))
                {
                    return utilisateur;
                }
            }
            return null;
        }

        public static bool ChangerMotDePasse(string identifiant, string ancienMotDePasse, string nouveauMotDePasse)
        {
            if (!_utilisateurs.TryGetValue(identifiant, out Utilisateur utilisateur))
            {
                return false;
            }

            if (!utilisateur.VerifierMotDePasse(ancienMotDePasse))
            {
                return false;
            }

            utilisateur.DefinirMotDePasse(nouveauMotDePasse);
            return true;
        }
    }
} 