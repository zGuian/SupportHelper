using Microsoft.AspNetCore.SignalR;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public abstract class BaseHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            //ADICIONAR LOGICA PARA PEGAR ID DA CONEXÃO
            return base.OnConnectedAsync();
        }
    }
}
