using LivinParis.Models;

namespace LivinParis.Tests
{
    /// <summary>
    /// Utility class providing test data and helper methods for unit tests.
    /// </summary>
    public static class TestUtils
    {
        /// <summary>
        /// Gets a test station instance with predefined values.
        /// </summary>
        public static Station StationTest => new Station(1, "Station Test", "Ligne 1", 48.8566, 2.3522);
    }
} 