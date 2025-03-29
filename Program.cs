using System;
using LivinParis.Models;

namespace LivinParis
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MenuPrincipal.Demarrer();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue : {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour quitter...");
                Console.ReadKey();
            }
        }
    }
}
