using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;

namespace LivinParis.Models.Trajets
{
    public class VisualisationReseau
    {
        private readonly ReseauMetro _reseau;
        private readonly int _width;
        private readonly int _height;
        private readonly float _scale;
        private readonly float _offsetX;
        private readonly float _offsetY;
        private List<Station> _trajet;

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

        private PointF ConvertirCoordonnees(double latitude, double longitude)
        {
            var x = (float)((longitude - _reseau.Stations.Values.Min(s => s.Longitude)) * _scale) + _offsetX;
            var y = (float)((_reseau.Stations.Values.Max(s => s.Latitude) - latitude) * _scale) + _offsetY;
            return new PointF(x, y);
        }

        public void DessinerTrajet(List<Station> trajet)
        {
            _trajet = trajet;
        }

        public void SauvegarderImage(string chemin)
        {
            try
            {
                using (var image = new Image<Rgba32>(_width, _height))
                {
                    // Dessiner le fond
                    image.Mutate(x => x.Fill(Color.White));

                    // Dessiner les lignes de métro en gris clair
                    foreach (var noeud in _reseau.Graphe.Noeuds.Values)
                    {
                        foreach (var voisin in noeud.Voisins)
                        {
                            var point1 = ConvertirCoordonnees(noeud.Valeur.Latitude, noeud.Valeur.Longitude);
                            var point2 = ConvertirCoordonnees(voisin.Key.Valeur.Latitude, voisin.Key.Valeur.Longitude);
                            image.Mutate(x => x.DrawLines(Color.LightGray, 1f, new[] { point1, point2 }));
                        }
                    }

                    // Dessiner les stations en bleu clair
                    foreach (var station in _reseau.Stations.Values)
                    {
                        var point = ConvertirCoordonnees(station.Latitude, station.Longitude);
                        var rect = new RectangleF(point.X - 3, point.Y - 3, 6, 6);
                        image.Mutate(x => x.Fill(Color.LightBlue, rect));
                    }

                    // Dessiner le trajet s'il existe
                    if (_trajet != null && _trajet.Count >= 2)
                    {
                        // Dessiner le trajet en rouge vif et plus épais
                        for (int i = 0; i < _trajet.Count - 1; i++)
                        {
                            var point1 = ConvertirCoordonnees(_trajet[i].Latitude, _trajet[i].Longitude);
                            var point2 = ConvertirCoordonnees(_trajet[i + 1].Latitude, _trajet[i + 1].Longitude);
                            image.Mutate(x => x.DrawLines(Color.Red, 5f, new[] { point1, point2 }));
                        }

                        // Mettre en évidence les stations du trajet en rouge vif
                        foreach (var station in _trajet)
                        {
                            var point = ConvertirCoordonnees(station.Latitude, station.Longitude);
                            var rect = new RectangleF(point.X - 6, point.Y - 6, 12, 12);
                            image.Mutate(x => x.Fill(Color.Red, rect));
                        }

                        // Ajouter les noms des stations du trajet en rouge
                        var font = SystemFonts.CreateFont("DejaVu Sans", 12);
                        foreach (var station in _trajet)
                        {
                            var point = ConvertirCoordonnees(station.Latitude, station.Longitude);
                            image.Mutate(x => x.DrawText(
                                station.Nom,
                                font,
                                Color.Red,
                                new PointF(point.X + 7, point.Y - 7)
                            ));
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

        private void OuvrirImage(string chemin)
        {
            try
            {
                string cheminAbsolu = Path.GetFullPath(chemin);
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // Essayer d'abord avec display (ImageMagick)
                    try
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "display",
                            Arguments = $"\"{cheminAbsolu}\"",
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        Process.Start(startInfo);
                    }
                    catch
                    {
                        // Si display échoue, essayer avec eog (Eye of GNOME)
                        try
                        {
                            var startInfo = new ProcessStartInfo
                            {
                                FileName = "eog",
                                Arguments = $"\"{cheminAbsolu}\"",
                                UseShellExecute = true,
                                CreateNoWindow = true
                            };
                            Process.Start(startInfo);
                        }
                        catch
                        {
                            // Si eog échoue, essayer avec feh
                            try
                            {
                                var startInfo = new ProcessStartInfo
                                {
                                    FileName = "feh",
                                    Arguments = $"\"{cheminAbsolu}\"",
                                    UseShellExecute = true,
                                    CreateNoWindow = true
                                };
                                Process.Start(startInfo);
                            }
                            catch
                            {
                                Console.WriteLine("Aucun visualiseur d'images n'a été trouvé. L'image a été sauvegardée ici :");
                                Console.WriteLine(cheminAbsolu);
                            }
                        }
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = cheminAbsolu,
                        UseShellExecute = true
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", cheminAbsolu);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Impossible d'ouvrir l'image : {ex.Message}");
                Console.WriteLine($"L'image a été sauvegardée ici : {Path.GetFullPath(chemin)}");
            }
        }
    }
} 