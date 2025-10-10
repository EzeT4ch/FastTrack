namespace Shared.Common.Result;

public class Result<T, E>
{
    private Result(
        E error)
    {
        Error = error;
        Message = string.Empty;
    }

    private Result(T data, string message = "")
    {
        Data = data;
        Message = message;
    }

    public T? Data { get; private set; }
        public E? Error { get; private set; }
    public string Message { get; private set; }
    private bool IsSuccess => Error == null;
    public bool IsFailure => !IsSuccess;

    public static Result<T, E> SetError(E error)
    {
        return new Result<T, E>(error);
    }

    public static Result<T, E> SetSuccess(T data, string message = "")
    {
        return new Result<T, E>(data, message);
    }
}