using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportHelper.Infrastructure.SignalR.Interfaces
{
    public interface IQueueProcess
    {
        (string requestId, string response) Dequeue();
        void Dequeue(out string requestId, out string response);
        void Enqueue(string requestId, string response);
        bool HasValue();
    }
}
