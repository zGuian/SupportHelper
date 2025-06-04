namespace SupportHelper.Communication.Responses
{
    public sealed class ResponseBase<T>
    {
        public bool IsSuccess { get; private set; }
        public string? MessageError { get; private set; }
        public T? Value { get; private set; }

        public ResponseBase(bool isSuccess, T? value)
        {
            IsSuccess = isSuccess;
            if (value != null)
            {
                Value = value;
            }
        }
    }
}
