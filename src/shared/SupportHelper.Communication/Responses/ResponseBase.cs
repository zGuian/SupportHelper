namespace SupportHelper.Communication.Responses
{
    public class ResponseBase<T> where T : class
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }

        private ResponseBase(bool isSuccess, T? value)
        {
            IsSuccess = isSuccess;
            if (value != null)
            {
                Value = value;
            }
        }

        public ResponseBase<T> Success(bool isSuccess, T value)
        {
            return new ResponseBase<T>(true, value);
        }

        public ResponseBase<T> NotSuccess(T? value)
        {
            return new ResponseBase<T>(false, value);
        }
    }
}
