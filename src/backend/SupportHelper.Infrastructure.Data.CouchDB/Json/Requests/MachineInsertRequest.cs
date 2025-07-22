using SupportHelper.Domain.Entities;
using SupportHelper.Domain.ValueObjects;

namespace SupportHelper.Infrastructure.Data.CouchDB.Json.Requests
{
    internal record MachineInsertRequest(Machine machine, SignalR SignalR);
}
