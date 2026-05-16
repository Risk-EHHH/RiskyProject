using System;
using System.Collections.Generic;

namespace Risk.Runtime.GameState
{
    [Serializable]
    public class PlayerState
    {
        public string Id;
        public string Name;
        public string Color;
        public List<string> Territories;
        public int ArmyPool;
        public bool IsWinner;
        public bool IsEliminated;
        public List<string> Eliminations;
    }
}