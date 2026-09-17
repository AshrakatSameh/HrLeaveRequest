namespace Hr.BlazorClient.Services;

public record ApiResult(bool IsSuccess, string? Error)
{
    public static ApiResult Success() => new(true, null);
    public static ApiResult Failure(string error) => new(false, error);
}

public record ApiResult<T>(bool IsSuccess, T? Value, string? Error)
{
    public static ApiResult<T> Success(T value) => new(true, value, null);
    public static ApiResult<T> Failure(string error) => new(false, default, error);
}
