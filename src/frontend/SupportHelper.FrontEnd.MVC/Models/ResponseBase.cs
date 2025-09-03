namespace SupportHelper.FrontEnd.MVC.Models
{
    public class ResponseBase<T> where T : class
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? Message { get; private set; }

        private ResponseBase(bool isSuccess, T? data, string? message = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }

        public static ResponseBase<T> ReturnSuccess(T data)
        {
            if (data != null)
            {
                return new ResponseBase<T>(true, data);
            }
            return ReturnFalse();
        }


        private static ResponseBase<T> ReturnFalse()
        {
            return new ResponseBase<T>(false, null);
        }

        public static ResponseBase<T> ReturnFalse(string message)
        {
            return new ResponseBase<T>(false, null, message);
        }
    }
}
