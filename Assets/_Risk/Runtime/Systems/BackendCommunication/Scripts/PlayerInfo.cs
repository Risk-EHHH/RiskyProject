using System;
using System.Collections.Generic;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    [Serializable]
    public class PlayerInfo
    {
        public string Id { get; set; } // Unique ID of this player.
        public string Name { get; set; } // Name of this player.
        public string Color { get; set; }  // Color of this player.
        public List<string> Territories { get; set; } // List of territories controlled by this player.
        public int ArmyPool { get; set; } // Number of troops available to this player for reinforcement.
        public bool IsEliminated { get; set; } // True if this player has been eliminated from the game.
    }
        
}
