namespace Shared.Common.Result
{
    public class Result<T, E> 
    {
        public T? Data { get; private set; }
        public E? Error { get; private set; }
        public string Message { get; private set; }
        private bool IsSuccess => Error == null;
        public bool IsFailure => !IsSuccess;

        private Result(
            E error, string message)
        {
            Error = error;
            Message = message;
        }

        private Result(T data, string message = "")
        {
            Data = data;
            Message = message;
        }

        public static Result<T, E> SetError(E error, string message)
            => new (error, message);

        public static Result<T, E> SetSuccess(T data, string message = "")
            => new (data, message);
    }
}