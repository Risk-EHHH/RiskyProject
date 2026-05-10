using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class PlayerInfo
    {
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
    }
}