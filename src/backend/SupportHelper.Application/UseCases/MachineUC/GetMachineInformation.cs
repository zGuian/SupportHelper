using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class GetMachineInformation : IGetMachineInformation
    {
        public GetMachineInformation() 
        {

        }

        public async ValueTask<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request)
        {
            throw new NotImplementedException();
        }

        private void ConvertToEntity()
        {
            throw new NotImplementedException();
        }
    }
}
