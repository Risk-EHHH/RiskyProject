using System;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class PlayerAction
    {
        [JsonProperty("action_id")] public string ActionId;
        [JsonProperty("error")] public string Error;
    }
}
