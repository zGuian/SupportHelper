namespace SupportHelper.Communication.Requests
{
    public sealed class RequestBase<T>(T entity) where T : class
    {
        public Guid IdRequest { get; } = new Guid();
        public T Entity { get; init; } = entity;
    }
}
