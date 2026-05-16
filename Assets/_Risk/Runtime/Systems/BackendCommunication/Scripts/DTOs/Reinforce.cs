using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class Reinforce
    {
        [JsonProperty("player_id")] public string PlayerId;
        [JsonProperty("action")] public string Action = "reinforce";
        [JsonProperty("payload")] public Payload Payload;
    }

    [Serializable]
    public class Payload
    {
        [JsonProperty("territories")] public List<ReinforceTerritory> Territories;
    }

    [Serializable]
    public class ReinforceTerritory
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("armies")] public int Armies;
    }
}