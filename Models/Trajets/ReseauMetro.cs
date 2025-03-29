using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LivinParis.Models.Trajets
{
    public class ReseauMetro
    {
        private Dictionary<int, Station> _stations;
        private Graphe<Station> _graphe;

        public IReadOnlyDictionary<int, Station> Stations => _stations;
        public Graphe<Station> Graphe => _graphe;

        public ReseauMetro()
        {
            _graphe = new Graphe<Station>();
            _stations = new Dictionary<int, Station>();
        }

        public void AjouterStation(Station station)
        {
            _stations[station.Id] = station;
            _graphe.AjouterNoeud(station);
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
    }
} 