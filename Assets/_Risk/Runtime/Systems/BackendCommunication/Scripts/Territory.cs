using System;
using System.Collections.Generic;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Represents a territory in the game.
    /// </summary>
    [Serializable]
    public class Territory
    {
        public string Name { get; set; } // Unique name of this territory (e.g., "Greenland").
        public List<string> Borders { get; set; } // Names of adjacent territories for movement/attacks.
        public string Owner { get; set; } // Player ID of the player currently occupying this territory.
        public int Armies { get; set; } // Number of troops stationed on this territory.
    }
}
