using System;
using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    [Serializable]
    public class ActionResult
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("action_status")]
        public string ActionStatus;
        [JsonProperty("result")]
        public string Result;
        [JsonProperty("error")]
        public ActionError Error;
    }

    [Serializable]
    public class ActionError
    {
        [JsonProperty("code")]
        public string Code;
        [JsonProperty("reason")]
        public string Reason;
    }
}