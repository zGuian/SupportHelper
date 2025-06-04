namespace SupportHelper.RabbitMQ
{
    public static class TaskExtensions
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, cts.Token);

            var completed = await Task.WhenAny(task, delayTask);
            if (completed == task)
            {
                cts.Cancel();
                return await task;
            }

            throw new TimeoutException("Tempo limite excedido para a resposta.");
        }
    }
}
