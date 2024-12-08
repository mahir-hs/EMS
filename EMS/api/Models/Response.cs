namespace api.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T? Result { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public dynamic? Data { get; set; }

        public Response(T? result, bool success, string message, string type = "General", dynamic? data = null)
        {
            Result = result;
            Success = success;
            Message = message;
            Type = type;
            Data = data;
        }

    }
}
