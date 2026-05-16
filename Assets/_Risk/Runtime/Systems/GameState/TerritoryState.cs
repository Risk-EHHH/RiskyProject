using System;

namespace Risk.Runtime.GameState
{
    [Serializable]
    public class TerritoryState
    {
        public string Name;
        public string Owner;
        public int Armies;
    }
}