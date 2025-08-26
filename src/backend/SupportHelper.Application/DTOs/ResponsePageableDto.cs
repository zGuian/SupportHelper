namespace SupportHelper.Application.DTOs
{
    public sealed record ResponsePageableDto<T> where T : class
    {
        public int TotalQuantity { get; init; }
        public int CurrentPage { get; init; }
        public int PageCount { get; init; }
        public T? Datas { get; init; }
    }
}
