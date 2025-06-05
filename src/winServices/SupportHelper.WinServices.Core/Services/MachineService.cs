using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.RabbitMQ.Interfaces;
using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Models;
using System.Text;
using System.Text.Json;

namespace SupportHelper.WinServices.Core.Services
{
    public sealed class MachineService : IMachineService
    {
        private readonly ILogger<MachineService> _logger;

        public MachineService(ILogger<MachineService> logger)
        {
            _logger = logger;
        }

        public MachineModel GetInformationMachine(CancellationToken cancellationToken = default)
        {
            var machine = new MachineModel();
            machine.GetAllInformationFromMachine();
            return machine;
        }
    }
}