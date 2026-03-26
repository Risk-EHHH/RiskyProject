using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class BoardInfoMetadata
    {
        [JsonProperty("continents")]
        public List<Continent> Continents;
    }

    [Serializable]
    public class Continent
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("territories")]
        public List<Territory> Territories;
    }

    [Serializable]
    public class Territory
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("borders")]
        public List<string> Borders;
        [JsonProperty("unit")]
        public string Unit;
        [JsonProperty("owner")]
        public string Owner;
        [JsonProperty("armies")]
        public int Armies;
    }
}