using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    public class ApiResponse<T>
    {
        [JsonProperty("status")]
        public string Status;
        [JsonProperty("message")]
        public string Message;
        [JsonProperty("metadata")]
        public T Metadata;
    }
}
