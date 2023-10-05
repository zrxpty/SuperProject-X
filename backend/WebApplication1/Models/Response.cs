namespace GATEWAY.Models
{
    public class Response<T> : ResponseStatus
    {
        public T? Data { get; set; } = default;
    }

    public class ResponseStatus
    {
        public int Code { get; set; } = 200;
        public string? Message { get; set; } = null;
    }
}
