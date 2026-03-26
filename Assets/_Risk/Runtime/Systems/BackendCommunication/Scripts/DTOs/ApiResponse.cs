namespace Risk.Runtime.BackendCommunication
{
    public class ApiResponse<T>
    {
        public string status;
        public string message;
        public T metadata;
    }
}
