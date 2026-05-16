using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class SecretPlayerInfo
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("color")]
        public string Color;
        [JsonProperty("territories")]
        public List<string> Territories;
        [JsonProperty("army_pool")]
        public int ArmyPool;
        [JsonProperty("is_winner")]
        public bool IsWinner;
        [JsonProperty("is_eliminated")]
        public bool IsEliminated;
        [JsonProperty("eliminations")]
        public List<string> Eliminations;
        [JsonProperty("mission")]
        public Mission Mission;
    }

    [Serializable]
    public class Mission
    {
        [JsonProperty("description")]
        public string Description;
        [JsonProperty("target")]
        public string Target;
        [JsonProperty("fallback_mission")]
        public FallbackMission FallbackMission;
    }

    [Serializable]
    public class FallbackMission
    {
        [JsonProperty("description")]
        public string Description;
        [JsonProperty("territory_count")]
        public int TerritoryCount;
        [JsonProperty("army_count")]
        public int ArmyCount;
    }
}