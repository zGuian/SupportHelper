using Newtonsoft.Json;

namespace SupportHelper.FrontEnd.MVC.DTOs
{
    public class JsonBaseDto<T>
    {
        public bool IsSuccess { get; set; }
        public string OnDate { get; set; } = string.Empty;
        public T? Data { get; set; }


        [JsonConstructor]
        public JsonBaseDto(bool isSuccess, string onDate, T? data)
        {
            IsSuccess = isSuccess;
            OnDate = onDate;
            Data = data;
        }
    }
}
