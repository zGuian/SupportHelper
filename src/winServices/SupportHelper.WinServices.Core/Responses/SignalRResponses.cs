using SupportHelper.WinServices.Application.Interfaces.Responses;
using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Responses
{
    public class SignalRResponses : ISignalRResponses
    {
        

        public SignalRResponses()
        {
            
        }

        public void Execute()
        {
            MachineModel machine = MachineModel.Create();

        }
    }
}
