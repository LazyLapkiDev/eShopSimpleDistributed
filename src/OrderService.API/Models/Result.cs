namespace OrdersService.API.Models;

public record Result<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }

    public Result(T? data)
    {
        Success = true;
        Data = data;
    }

    public Result(string error)
    {
        Success = false;
        Error = error;
    }

    public Result(bool succes, T? data)
    {
        Success = succes;
        Data = data;
    }

    public Result(bool succes, string error)
    {
        Success = succes;
        Error = error;
    }
}
