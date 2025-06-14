using SupportHelper.Communication.Responses;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : BaseHub
    {
        private readonly IConnectionService _connectionService;

        public ControlHub(IConnectionService connectionService) : base(connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task ResponseStatusMachine(MachineInformationResponse response) 
        {
            //ADICIONAR LOGICA PARA PERSISTIR DADOS
            await Task.CompletedTask;
        }
    }
}
