using System;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class NewGameMetadata
    {
        [JsonProperty("game_id")]
        public string GameID;
    }
}
