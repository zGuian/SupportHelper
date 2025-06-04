namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IReplyAwaiter<T> where T : class
    {
        Task<T> WaitForResponseAsync(Guid correlationId, TimeSpan? timeout = null);
        void SetResponse(string correlationId, T response);
        void SetException(string correlationId, Exception ex);
    }
}
