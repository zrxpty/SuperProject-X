namespace Tools.GenericModels
{
    public class ServiceResponse<T> : ServiceStatus
    {
        public T? Data { get; set; } = default;
    }

    public class ServiceStatus
    {
        public int Code { get; set; } = 200;
        public string? Message { get; set; } = null;
    }
}