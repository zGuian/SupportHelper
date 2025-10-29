namespace SupportHelper.API.Domain.DTOs.Responses
{
    public sealed class ResponseBase<T>
    {
        public bool IsSuccess { get; private set; }
        public string? MessageError { get; private set; }
        public T? Value { get; private set; }

        public ResponseBase(T value)
        {
            IsSuccess = true;
            MessageError = null;
            Value = value;
        }

        public ResponseBase(string message)
        {
            IsSuccess = false;
            MessageError = message;
            Value = default;
        }

        public static class Factories
        {
            public static ResponseBase<T> Success(T value)
            {
                return new ResponseBase<T>(value);
            }

            public static ResponseBase<T> Error(string message)
            {
                return new ResponseBase<T>(message);
            }
        }
    }
}
