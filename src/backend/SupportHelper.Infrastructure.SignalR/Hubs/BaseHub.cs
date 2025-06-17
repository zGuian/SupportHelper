using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    //public abstract class BaseHub : Hub
    //{
    //    private readonly IConnectionService _connectionService;

    //    protected BaseHub(IConnectionService connectionService)
    //    {
    //        _connectionService = connectionService;
    //    }

    //    public async override Task OnConnectedAsync()
    //    {
    //        HttpContext httpContext = Context.GetHttpContext() ?? throw new Exception("NÃO ENCONTRADO VALORES DE URL");

    //        string hostName = httpContext.Request.Query["hostname"].ToString().ToLower();
    //        string connId = Context.ConnectionId;
    //        Console.WriteLine(hostName);
    //        _connectionService.Register(hostName, connId);
    //        await base.OnConnectedAsync();
    //    }

    //    public override Task OnDisconnectedAsync(Exception? exception)
    //    {
    //        Context.Abort();
    //        return base.OnDisconnectedAsync(exception);
    //    }
    //}
}
