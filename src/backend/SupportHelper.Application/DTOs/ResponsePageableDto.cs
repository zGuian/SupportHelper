namespace SupportHelper.Application.DTOs
{
    public sealed class ResponsePageableDto<T>
    {
        public int TotalQuantity { get; init; }
        public int CurrentPage { get; init; }
        public int PageCount { get; init; }
        public T Datas { get; init; }
    }
}
