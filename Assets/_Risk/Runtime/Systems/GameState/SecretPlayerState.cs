using System;
using System.Collections.Generic;

namespace Risk.Runtime.GameState
{
    [Serializable]
    public class SecretPlayerState
    {
        public string Id;
        public string Name;
        public string Color;
        public List<string> Territories;
        public int ArmyPool;
        public bool IsWinner;
        public bool IsEliminated;
        public List<string> Eliminations;
        public MissionState Mission;
    }

    [Serializable]
    public class MissionState
    {
        public string Description;
        public string Target;
        public FallbackMissionState FallbackMission;
    }

    [Serializable]
    public class FallbackMissionState
    {
        public string Description;
        public int TerritoryCount;
        public int ArmyCount;
    }
}