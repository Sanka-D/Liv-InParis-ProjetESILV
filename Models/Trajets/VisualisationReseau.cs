using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using SDColor = System.Drawing.Color;
using SDPointF = System.Drawing.PointF;
using SDRectangleF = System.Drawing.RectangleF;
using ImgColor = SixLabors.ImageSharp.Color;
using ImgPointF = SixLabors.ImageSharp.PointF;
using ImgRectangleF = SixLabors.ImageSharp.RectangleF;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;

namespace LivinParis.Models.Trajets
{
    /// <summary>
    /// Provides visualization capabilities for the metro network.
    /// </summary>
    public class VisualisationReseau
    {
        private readonly ReseauMetro _reseau;
        private readonly int _width;
        private readonly int _height;
        private readonly float _scale;
        private readonly float _offsetX;
        private readonly float _offsetY;
        private List<Station> _trajet;

        /// <summary>
        /// Initializes a new instance of the VisualisationReseau class.
        /// </summary>
        public VisualisationReseau(ReseauMetro reseau, int width = 1200, int height = 800)
        {
            _reseau = reseau;
            _width = width;
            _height = height;

            // Calculer les limites du réseau
            var minLat = _reseau.Stations.Values.Min(s => s.Latitude);
            var maxLat = _reseau.Stations.Values.Max(s => s.Latitude);
            var minLon = _reseau.Stations.Values.Min(s => s.Longitude);
            var maxLon = _reseau.Stations.Values.Max(s => s.Longitude);

            // Calculer l'échelle et les offsets pour adapter le réseau à la fenêtre
            _scale = (float)Math.Min(
                _width / (maxLon - minLon),
                _height / (maxLat - minLat)
            ) * 0.9f; // 90% de la taille pour avoir une marge

            _offsetX = (_width - (float)(maxLon - minLon) * _scale) / 2;
            _offsetY = (_height - (float)(maxLat - minLat) * _scale) / 2;
        }

        /// <summary>
        /// Converts geographical coordinates to image coordinates.
        /// </summary>
        private ImgPointF ConvertirCoordonnees(double latitude, double longitude)
        {
            var x = (float)((longitude - _reseau.Stations.Values.Min(s => s.Longitude)) * _scale) + _offsetX;
            var y = (float)((_reseau.Stations.Values.Max(s => s.Latitude) - latitude) * _scale) + _offsetY;
            return new ImgPointF(x, y);
        }

        /// <summary>
        /// Sets the path to be drawn.
        /// </summary>
        public void DessinerTrajet(List<Station> trajet)
        {
            _trajet = trajet;
        }

        /// <summary>
        /// Draws the graph with node coloring.
        /// </summary>
        public void DessinerColoration(Dictionary<Noeud<Station>, int> coloring, string chemin)
        {
            using (var image = new Image<Rgba32>(_width, _height))
            {
                image.Mutate(x => x.Fill(ImgColor.White));

                // Define colors for the graph
                var colors = new[]
                {
                    ImgColor.Red,
                    ImgColor.Blue,
                    ImgColor.Green,
                    ImgColor.Yellow,
                    ImgColor.Purple,
                    ImgColor.Orange,
                    ImgColor.Cyan,
                    ImgColor.Magenta,
                    ImgColor.LimeGreen,
                    ImgColor.HotPink
                };

                // Draw edges first
                foreach (var noeud in _reseau.Graphe.Noeuds.Values)
                {
                    var point1 = ConvertirCoordonnees(noeud.Valeur.Latitude, noeud.Valeur.Longitude);
                    foreach (var voisin in noeud.Voisins)
                    {
                        var point2 = ConvertirCoordonnees(voisin.Key.Valeur.Latitude, voisin.Key.Valeur.Longitude);
                        var pen = new SolidPen(ImgColor.Gray, 2f);
                        image.Mutate(x => x.DrawLine(pen, point1, point2));
                    }
                }

                // Draw nodes with their assigned colors
                foreach (var noeud in _reseau.Graphe.Noeuds.Values)
                {
                    var point = ConvertirCoordonnees(noeud.Valeur.Latitude, noeud.Valeur.Longitude);
                    var color = colors[coloring[noeud] % colors.Length];
                    var rect = new ImgRectangleF(point.X - 8, point.Y - 8, 16, 16);
                    image.Mutate(x => x.Fill(color, rect));
                }

                // Add node labels
                var font = SystemFonts.Get("DejaVu Sans").CreateFont(12);
                foreach (var noeud in _reseau.Graphe.Noeuds.Values)
                {
                    var point = ConvertirCoordonnees(noeud.Valeur.Latitude, noeud.Valeur.Longitude);
                    var options = new RichTextOptions(font)
                    {
                        Origin = new ImgPointF(point.X + 10, point.Y - 10)
                    };
                    image.Mutate(x => x.DrawText(options, noeud.Valeur.Nom, ImgColor.Black));
                }

                image.Save(chemin);
            }
        }

        /// <summary>
        /// Saves the network visualization to an image file.
        /// </summary>
        public void SauvegarderImage(string chemin)
        {
            try
            {
                using (var image = new Image<Rgba32>(_width, _height))
                {
                    // Dessiner le fond
                    image.Mutate(x => x.Fill(ImgColor.White));

                    // Dessiner les lignes de métro en gris clair
                    foreach (var noeud in _reseau.Graphe.Noeuds.Values)
                    {
                        foreach (var voisin in noeud.Voisins)
                        {
                            var point1 = ConvertirCoordonnees(noeud.Valeur.Latitude, noeud.Valeur.Longitude);
                            var point2 = ConvertirCoordonnees(voisin.Key.Valeur.Latitude, voisin.Key.Valeur.Longitude);
                            var pen = new SolidPen(ImgColor.LightGray, 1f);
                            image.Mutate(x => x.DrawLine(pen, point1, point2));
                        }
                    }

                    // Dessiner les stations en bleu clair
                    foreach (var station in _reseau.Stations.Values)
                    {
                        var point = ConvertirCoordonnees(station.Latitude, station.Longitude);
                        var rect = new ImgRectangleF(point.X - 3, point.Y - 3, 6, 6);
                        image.Mutate(x => x.Fill(ImgColor.LightBlue, rect));
                    }

                    // Dessiner le trajet s'il existe
                    if (_trajet != null && _trajet.Count >= 2)
                    {
                        // Dessiner le trajet en rouge vif et plus épais
                        for (int i = 0; i < _trajet.Count - 1; i++)
                        {
                            var point1 = ConvertirCoordonnees(_trajet[i].Latitude, _trajet[i].Longitude);
                            var point2 = ConvertirCoordonnees(_trajet[i + 1].Latitude, _trajet[i + 1].Longitude);
                            var pen = new SolidPen(ImgColor.Red, 5f);
                            image.Mutate(x => x.DrawLine(pen, point1, point2));
                        }

                        // Mettre en évidence les stations du trajet en rouge vif
                        foreach (var station in _trajet)
                        {
                            var point = ConvertirCoordonnees(station.Latitude, station.Longitude);
                            var rect = new ImgRectangleF(point.X - 6, point.Y - 6, 12, 12);
                            image.Mutate(x => x.Fill(ImgColor.Red, rect));
                        }

                        // Ajouter les noms des stations du trajet en rouge
                        var font = SystemFonts.Get("DejaVu Sans").CreateFont(12);
                        foreach (var station in _trajet)
                        {
                            var point = ConvertirCoordonnees(station.Latitude, station.Longitude);
                            var options = new RichTextOptions(font)
                            {
                                Origin = new ImgPointF(point.X + 7, point.Y - 7)
                            };
                            image.Mutate(x => x.DrawText(options, station.Nom, ImgColor.Red));
                        }
                    }

                    // Sauvegarder l'image
                    image.Save(chemin);

                    // Ouvrir l'image
                    OuvrirImage(chemin);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de l'image : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Opens the generated image using the appropriate viewer for the current platform.
        /// </summary>
        private void OuvrirImage(string chemin)
        {
            try
            {
                string cheminAbsolu = System.IO.Path.GetFullPath(chemin);
                string cheminUrl = $"file://{cheminAbsolu}";
                
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c start {cheminUrl}",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "xdg-open",
                        Arguments = cheminUrl,
                        UseShellExecute = true,
                        CreateNoWindow = true
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", cheminUrl);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Impossible d'ouvrir l'image : {ex.Message}");
                Console.WriteLine($"L'image a été sauvegardée ici : {System.IO.Path.GetFullPath(chemin)}");
            }
        }
    }
} 