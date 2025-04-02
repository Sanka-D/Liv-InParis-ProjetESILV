using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LivinParis.Models.Trajets
{
    public class ReseauMetro
    {
        private static ReseauMetro _instance;
        private Dictionary<int, Station> _stations;
        private Graphe<Station> _graphe;

        public static ReseauMetro Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReseauMetro();
                }
                return _instance;
            }
        }

        public IReadOnlyDictionary<int, Station> Stations => _stations;
        public Graphe<Station> Graphe => _graphe;

        internal ReseauMetro()
        {
            _stations = new Dictionary<int, Station>();
            _graphe = new Graphe<Station>();
        }

        public void AjouterStation(Station station)
        {
            if (!_stations.ContainsKey(station.Id))
            {
                _stations.Add(station.Id, station);
                _graphe.AjouterSommet(station);
            }
        }

        public void AjouterLigne(Ligne ligne)
        {
            if (!_stations.ContainsKey(ligne.StationDepart.Id) || !_stations.ContainsKey(ligne.StationArrivee.Id))
                throw new Exception("Les stations de la ligne doivent exister dans le réseau");

            _graphe.AjouterArete(ligne.StationDepart, ligne.StationArrivee, ligne.Duree);
        }

        public void AjouterConnexion(int station1Id, int station2Id)
        {
            if (!_stations.ContainsKey(station1Id) || !_stations.ContainsKey(station2Id))
                throw new ArgumentException("Les stations doivent exister dans le réseau");

            var station1 = _stations[station1Id];
            var station2 = _stations[station2Id];
            var distance = station1.CalculerDistance(station2);

            _graphe.AjouterArete(station1, station2, distance);
        }

        public Station ObtenirStation(int id)
        {
            return _stations.TryGetValue(id, out var station) ? station : null;
        }

        public void ComparerAlgorithmes(int departId, int arriveeId)
        {
            var depart = ObtenirStation(departId);
            var arrivee = ObtenirStation(arriveeId);

            if (depart == null || arrivee == null)
                throw new ArgumentException("Les stations de départ et d'arrivée doivent exister");

            Console.WriteLine($"\nComparaison des algorithmes de plus court chemin");
            Console.WriteLine($"De : {depart}");
            Console.WriteLine($"À : {arrivee}");
            Console.WriteLine("================================================");

            // Dijkstra
            var sw = Stopwatch.StartNew();
            var cheminDijkstra = _graphe.Dijkstra(depart, arrivee);
            sw.Stop();
            var distanceDijkstra = _graphe.CalculerDistanceTotale(cheminDijkstra);
            Console.WriteLine($"\nDijkstra :");
            Console.WriteLine($"Temps d'exécution : {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Distance totale : {distanceDijkstra:F2} km");
            AfficherChemin(cheminDijkstra);

            // Bellman-Ford
            sw.Restart();
            var cheminBellmanFord = _graphe.BellmanFord(depart, arrivee);
            sw.Stop();
            var distanceBellmanFord = _graphe.CalculerDistanceTotale(cheminBellmanFord);
            Console.WriteLine($"\nBellman-Ford :");
            Console.WriteLine($"Temps d'exécution : {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Distance totale : {distanceBellmanFord:F2} km");
            AfficherChemin(cheminBellmanFord);

            // Floyd-Warshall
            sw.Restart();
            var cheminFloydWarshall = _graphe.FloydWarshall(depart, arrivee);
            sw.Stop();
            var distanceFloydWarshall = _graphe.CalculerDistanceTotale(cheminFloydWarshall);
            Console.WriteLine($"\nFloyd-Warshall :");
            Console.WriteLine($"Temps d'exécution : {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Distance totale : {distanceFloydWarshall:F2} km");
            AfficherChemin(cheminFloydWarshall);
        }

        private void AfficherChemin(List<Station> chemin)
        {
            Console.WriteLine("Itinéraire :");
            for (int i = 0; i < chemin.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {chemin[i]}");
            }
        }

        public Station RechercherStationParNom(string nomStation)
        {
            return Stations.Values.FirstOrDefault(s => 
                s.Nom.ToLower().Contains(nomStation.ToLower()));
        }

        public List<Station> RechercherStationsParNom(string nomStation)
        {
            return Stations.Values
                .Where(s => s.Nom.ToLower().Contains(nomStation.ToLower()))
                .ToList();
        }

        public List<Station> ObtenirTrajet(int stationDepartId, int stationArriveeId)
        {
            if (!_stations.ContainsKey(stationDepartId) || !_stations.ContainsKey(stationArriveeId))
                throw new Exception("Les stations spécifiées n'existent pas dans le réseau");

            var stationDepart = _stations[stationDepartId];
            var stationArrivee = _stations[stationArriveeId];

            var chemin = _graphe.TrouverCheminLePlusCourt(stationDepart, stationArrivee);
            return chemin;
        }

        public List<Station> ObtenirStations()
        {
            return _stations.Values.ToList();
        }

        public List<Ligne> ObtenirLignes()
        {
            var aretes = _graphe.ObtenirAretes();
            return aretes.Select(a => new Ligne(
                a.Depart.Id, 
                $"Ligne {a.Depart.Id}-{a.Arrivee.Id}", 
                a.Depart, 
                a.Arrivee, 
                a.Distance
            )).ToList();
        }
    }
} 