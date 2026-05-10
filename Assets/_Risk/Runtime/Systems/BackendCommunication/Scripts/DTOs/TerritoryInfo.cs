using System;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class TerritoryInfo
    {
        [JsonProperty("owner")]
        public string Owner;
        [JsonProperty("armies")]
        public int Armies;
    }
}