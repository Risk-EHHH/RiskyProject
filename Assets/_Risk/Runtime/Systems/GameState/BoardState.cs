using System;
using System.Collections.Generic;

namespace Risk.Runtime.GameState
{
    [Serializable]
    public class BoardState
    {
        public List<ContinentState> Continents;
    }

    [Serializable]
    public class ContinentState
    {
        public string Name;
        public List<TerritorySetupState> Territories;
    }

    [Serializable]
    public class TerritorySetupState
    {
        public string Name;
        public List<string> Borders;
        public string Unit;
        public string Owner;
        public int Armies;
    }
}