using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportHelper.Infrastructure.Contracts
{
    public interface IMachineConsumer
    {
        Task ListenRabbitQueueDefault(IConfiguration configuration, CancellationToken cancellationToken);
    }
}
