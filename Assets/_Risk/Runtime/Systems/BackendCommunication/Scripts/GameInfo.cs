using System;
using System.Collections.Generic;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Represents the state of the game, including territories and players.
    /// </summary>
    [Serializable]
    public class GameInfo
    {
        public List<Territory> Territories { get; set; } // list of all territories status
        public List<string> Players { get; set; } // list of all players IDs
    }
}
