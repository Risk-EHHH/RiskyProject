using Newtonsoft.Json;

namespace Risk.Runtime.BackendCommunication
{
    /// <summary>
    /// Represents the response from the backend API. Contains status, message, and metadata.
    /// The metadata is the actual data returned by the backend, such as BoardInfo, PlayerInfo, etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
