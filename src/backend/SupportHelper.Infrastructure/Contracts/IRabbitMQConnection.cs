using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportHelper.Infrastructure.Contracts
{
    public interface IRabbitMQConnection
    {
        Task<IChannel> DeclareExchangeAndQueueDefaultAsync(CancellationToken cancellationToken = default);
    }
}
