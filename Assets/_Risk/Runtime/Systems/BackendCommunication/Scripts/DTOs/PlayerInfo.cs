using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    [Serializable]
    public class PlayerInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; } // Unique ID of this player.
        [JsonProperty("name")]
        public string Name { get; set; } // Name of this player.
        [JsonProperty("color")]
        public string Color { get; set; }  // Color of this player.
        [JsonProperty("territories")]
        public List<string> Territories { get; set; } // List of territories controlled by this player.
        [JsonProperty("army_pool")]
        public int ArmyPool { get; set; } // Number of troops available to this player for reinforcement.
        [JsonProperty("is_eliminated")]
        public bool IsEliminated { get; set; } // True if this player has been eliminated from the game.
        
        public override string ToString()
        {
            return $"PlayerInfo(Id='{Id}', Name='{Name}', Color='{Color}', Territories=[{string.Join(", ", Territories ?? new List<string>())}], ArmyPool={ArmyPool}, Eliminated={IsEliminated})";
        }
    }
        
}
