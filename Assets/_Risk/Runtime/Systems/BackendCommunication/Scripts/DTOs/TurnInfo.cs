using System;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class TurnInfo
    {
        [JsonProperty("current_player")]
        public string CurrentPlayer;
        [JsonProperty("current_phase")]
        public string CurrentPhase;
    }
}
