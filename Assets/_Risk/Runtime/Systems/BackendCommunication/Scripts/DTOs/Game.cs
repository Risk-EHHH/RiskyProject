using System;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class Game
    {
        [JsonProperty("game_id")]
        public string GameID;
    }
}
