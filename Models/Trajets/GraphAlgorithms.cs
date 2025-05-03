using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LivinParis.Models.Trajets
{
    /// <summary>
    /// Provides graph algorithms for the metro network.
    /// </summary>
    public static class GraphAlgorithms
    {
        /// <summary>
        /// Applies the Welsh-Powell algorithm to color the graph.
        /// </summary>
        public static Dictionary<Noeud<Station>, int> WelshPowellColoring(Graphe<Station> graphe)
        {
            var coloring = new Dictionary<Noeud<Station>, int>();
            var nodes = graphe.Noeuds.Values.OrderByDescending(n => n.Voisins.Count).ToList();
            int color = 0;

            while (nodes.Any())
            {
                var currentColor = color++;
                var coloredNodes = new List<Noeud<Station>>();

                foreach (var node in nodes.ToList())
                {
                    if (!coloredNodes.Any(n => n.Voisins.ContainsKey(node)))
                    {
                        coloring[node] = currentColor;
                        coloredNodes.Add(node);
                        nodes.Remove(node);
                    }
                }
            }

            return coloring;
        }

        /// <summary>
        /// Checks if the graph is bipartite based on its coloring.
        /// </summary>
        public static bool IsBipartite(Dictionary<Noeud<Station>, int> coloring)
        {
            return coloring.Values.Distinct().Count() <= 2;
        }

        /// <summary>
        /// Checks if the graph is planar using Euler's formula.
        /// </summary>
        public static bool IsPlanar(Graphe<Station> graphe)
        {
            int n = graphe.Noeuds.Count;
            int m = graphe.Noeuds.Values.Sum(node => node.Voisins.Count) / 2;
            return m <= 3 * n - 6;
        }

        /// <summary>
        /// Gets independent groups of nodes (nodes with the same color).
        /// </summary>
        public static Dictionary<int, List<Noeud<Station>>> GetIndependentGroups(Dictionary<Noeud<Station>, int> coloring)
        {
            return coloring
                .GroupBy(kvp => kvp.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(kvp => kvp.Key).ToList()
                );
        }

        /// <summary>
        /// Exports the graph coloring to a JSON file.
        /// </summary>
        public static void ExportToJson(Graphe<Station> graphe, Dictionary<Noeud<Station>, int> coloring, string filePath)
        {
            var exportData = new
            {
                Nodes = graphe.Noeuds.Values.Select(n => new
                {
                    Id = n.Valeur.Id,
                    Name = n.Valeur.Nom,
                    Color = coloring[n]
                }),
                Edges = graphe.Noeuds.Values.SelectMany(n => n.Voisins.Select(v => new
                {
                    From = n.Valeur.Id,
                    To = v.Key.Valeur.Id,
                    Weight = v.Value
                }))
            };

            string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Exports the graph coloring to an XML file.
        /// </summary>
        public static void ExportToXml(Graphe<Station> graphe, Dictionary<Noeud<Station>, int> coloring, string filePath)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("Graph");
            doc.AppendChild(root);

            var nodesElement = doc.CreateElement("Nodes");
            root.AppendChild(nodesElement);

            foreach (var node in graphe.Noeuds.Values)
            {
                var nodeElement = doc.CreateElement("Node");
                nodeElement.SetAttribute("Id", node.Valeur.Id.ToString());
                nodeElement.SetAttribute("Name", node.Valeur.Nom);
                nodeElement.SetAttribute("Color", coloring[node].ToString());
                nodesElement.AppendChild(nodeElement);
            }

            var edgesElement = doc.CreateElement("Edges");
            root.AppendChild(edgesElement);

            foreach (var node in graphe.Noeuds.Values)
            {
                foreach (var neighbor in node.Voisins)
                {
                    var edgeElement = doc.CreateElement("Edge");
                    edgeElement.SetAttribute("From", node.Valeur.Id.ToString());
                    edgeElement.SetAttribute("To", neighbor.Key.Valeur.Id.ToString());
                    edgeElement.SetAttribute("Weight", neighbor.Value.ToString());
                    edgesElement.AppendChild(edgeElement);
                }
            }

            doc.Save(filePath);
        }

        /// <summary>
        /// Implements the Chu-Liu/Edmonds algorithm for finding the minimum spanning arborescence.
        /// </summary>
        public static List<(Station From, Station To, double Weight)> ChuLiuEdmonds(
            List<Station> nodes,
            List<(Station From, Station To, double Weight)> edges,
            Station root)
        {
            var result = new List<(Station From, Station To, double Weight)>();
            var inEdges = new Dictionary<Station, (Station From, double Weight)>();
            var cycles = new List<List<Station>>();

            foreach (var node in nodes.Where(n => n != root))
            {
                var minEdge = edges
                    .Where(e => e.To == node)
                    .OrderBy(e => e.Weight)
                    .FirstOrDefault();

                if (minEdge.From != null)
                {
                    inEdges[node] = (minEdge.From, minEdge.Weight);
                }
            }

            var visited = new HashSet<Station>();
            foreach (var node in nodes)
            {
                if (visited.Contains(node)) continue;

                var cycle = new List<Station>();
                var current = node;
                while (current != null && !cycle.Contains(current))
                {
                    cycle.Add(current);
                    visited.Add(current);
                    current = inEdges.ContainsKey(current) ? inEdges[current].From : null;
                }

                if (current != null)
                {
                    var cycleStart = cycle.IndexOf(current);
                    cycles.Add(cycle.Skip(cycleStart).ToList());
                }
            }

            if (!cycles.Any())
            {
                foreach (var kvp in inEdges)
                {
                    result.Add((kvp.Value.From, kvp.Key, kvp.Value.Weight));
                }
                return result;
            }

            var contractedNodes = new List<Station>();
            var contractedEdges = new List<(Station From, Station To, double Weight)>();
            var cycleMap = new Dictionary<Station, Station>();

            foreach (var cycle in cycles)
            {
                var superNode = new Station(-cycle[0].Id, $"SuperNode_{cycle[0].Id}", "Ligne virtuelle", 0, 0);
                contractedNodes.Add(superNode);

                foreach (var node in cycle)
                {
                    cycleMap[node] = superNode;
                }
            }

            foreach (var node in nodes.Where(n => !cycles.Any(c => c.Contains(n))))
            {
                contractedNodes.Add(node);
                cycleMap[node] = node;
            }

            foreach (var edge in edges)
            {
                var from = cycleMap[edge.From];
                var to = cycleMap[edge.To];

                if (from != to)
                {
                    contractedEdges.Add((from, to, edge.Weight));
                }
            }

            var contractedArborescence = ChuLiuEdmonds(contractedNodes, contractedEdges, root);

            foreach (var edge in contractedArborescence)
            {
                if (edge.From.Id < 0)
                {
                    var cycle = cycles.First(c => c[0].Id == -edge.From.Id);
                    var minEdge = edges
                        .Where(e => cycle.Contains(e.From) && e.To == edge.To)
                        .OrderBy(e => e.Weight)
                        .First();
                    result.Add(minEdge);
                }
                else
                {
                    result.Add(edge);
                }
            }

            return result;
        }
    }
} 