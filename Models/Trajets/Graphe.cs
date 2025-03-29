using System;
using System.Collections.Generic;
using System.Linq;

namespace LivinParis.Models.Trajets
{
    public class Noeud<T>
    {
        public T Valeur { get; set; }
        public Dictionary<Noeud<T>, double> Voisins { get; set; }

        public Noeud(T valeur)
        {
            Valeur = valeur;
            Voisins = new Dictionary<Noeud<T>, double>();
        }

        public void AjouterVoisin(Noeud<T> voisin, double distance)
        {
            Voisins[voisin] = distance;
        }
    }

    public class Graphe<T>
    {
        private Dictionary<T, Noeud<T>> _noeuds;

        public IReadOnlyDictionary<T, Noeud<T>> Noeuds => _noeuds;

        public Graphe()
        {
            _noeuds = new Dictionary<T, Noeud<T>>();
        }

        public void AjouterNoeud(T valeur)
        {
            if (!_noeuds.ContainsKey(valeur))
            {
                _noeuds[valeur] = new Noeud<T>(valeur);
            }
        }

        public void AjouterArete(T depart, T arrivee, double distance)
        {
            if (!_noeuds.ContainsKey(depart) || !_noeuds.ContainsKey(arrivee))
                throw new ArgumentException("Les nœuds doivent exister dans le graphe");

            _noeuds[depart].AjouterVoisin(_noeuds[arrivee], distance);
            _noeuds[arrivee].AjouterVoisin(_noeuds[depart], distance); // Graphe non orienté
        }

        public List<T> Dijkstra(T depart, T arrivee)
        {
            var distances = new Dictionary<T, double>();
            var predecesseurs = new Dictionary<T, T>();
            var nonVisites = new HashSet<T>();

            foreach (var noeud in _noeuds.Keys)
            {
                distances[noeud] = double.MaxValue;
                nonVisites.Add(noeud);
            }

            distances[depart] = 0;

            while (nonVisites.Count > 0)
            {
                var u = nonVisites.OrderBy(n => distances[n]).First();
                nonVisites.Remove(u);

                if (u.Equals(arrivee)) break;

                foreach (var voisin in _noeuds[u].Voisins)
                {
                    var v = voisin.Key.Valeur;
                    var distance = voisin.Value;

                    if (distances[u] + distance < distances[v])
                    {
                        distances[v] = distances[u] + distance;
                        predecesseurs[v] = u;
                    }
                }
            }

            return ReconstruireChemin(predecesseurs, depart, arrivee);
        }

        public List<T> BellmanFord(T depart, T arrivee)
        {
            var distances = new Dictionary<T, double>();
            var predecesseurs = new Dictionary<T, T>();

            foreach (var noeud in _noeuds.Keys)
            {
                distances[noeud] = double.MaxValue;
            }

            distances[depart] = 0;

            for (int i = 0; i < _noeuds.Count - 1; i++)
            {
                foreach (var noeud in _noeuds.Values)
                {
                    foreach (var voisin in noeud.Voisins)
                    {
                        var u = noeud.Valeur;
                        var v = voisin.Key.Valeur;
                        var distance = voisin.Value;

                        if (distances[u] + distance < distances[v])
                        {
                            distances[v] = distances[u] + distance;
                            predecesseurs[v] = u;
                        }
                    }
                }
            }

            return ReconstruireChemin(predecesseurs, depart, arrivee);
        }

        public List<T> FloydWarshall(T depart, T arrivee)
        {
            var n = _noeuds.Count;
            var distances = new double[n, n];
            var next = new T[n, n];
            var noeuds = _noeuds.Keys.ToList();

            // Initialisation
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        distances[i, j] = 0;
                    else
                        distances[i, j] = double.MaxValue;
                }
            }

            // Remplissage des distances directes
            foreach (var noeud in _noeuds.Values)
            {
                var i = noeuds.IndexOf(noeud.Valeur);
                foreach (var voisin in noeud.Voisins)
                {
                    var j = noeuds.IndexOf(voisin.Key.Valeur);
                    distances[i, j] = voisin.Value;
                    next[i, j] = voisin.Key.Valeur;
                }
            }

            // Algorithme Floyd-Warshall
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }

            // Reconstruction du chemin
            var chemin = new List<T>();
            var debut = noeuds.IndexOf(depart);
            var fin = noeuds.IndexOf(arrivee);

            if (distances[debut, fin] == double.MaxValue)
                return chemin;

            chemin.Add(depart);
            while (!depart.Equals(arrivee))
            {
                depart = next[debut, fin];
                chemin.Add(depart);
                debut = noeuds.IndexOf(depart);
            }

            return chemin;
        }

        private List<T> ReconstruireChemin(Dictionary<T, T> predecesseurs, T depart, T arrivee)
        {
            var chemin = new List<T>();
            var courant = arrivee;

            while (predecesseurs.ContainsKey(courant))
            {
                chemin.Insert(0, courant);
                courant = predecesseurs[courant];
            }

            if (courant.Equals(depart))
            {
                chemin.Insert(0, depart);
            }

            return chemin;
        }

        public double CalculerDistanceTotale(List<T> chemin)
        {
            double distance = 0;
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                var noeud1 = _noeuds[chemin[i]];
                var noeud2 = _noeuds[chemin[i + 1]];
                distance += noeud1.Voisins[noeud2];
            }
            return distance;
        }
    }
} 