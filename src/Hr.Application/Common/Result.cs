namespace Hr.Application.Common;
// return results with no value, just success or error
public sealed record Result(ErrorType ErrorType, string? Error)
{
    public bool IsSuccess => ErrorType == ErrorType.None;

    public static Result Success() => new(ErrorType.None, null);
    public static Result Validation(string error) => new(ErrorType.Validation, error);
    public static Result NotFound(string error) => new(ErrorType.NotFound, error);
    public static Result Conflict(string error) => new(ErrorType.Conflict, error);
    public static Result Upstream(string error) => new(ErrorType.Upstream, error);
}

// return results with a value, or an error
public sealed record Result<T>(ErrorType ErrorType, T? Value, string? Error)
{
    public bool IsSuccess => ErrorType == ErrorType.None;

    public static Result<T> Success(T value) => new(ErrorType.None, value, null);
    public static Result<T> Validation(string error) => new(ErrorType.Validation, default, error);
    public static Result<T> NotFound(string error) => new(ErrorType.NotFound, default, error);
    public static Result<T> Conflict(string error) => new(ErrorType.Conflict, default, error);
    public static Result<T> Upstream(string error) => new(ErrorType.Upstream, default, error);
}
