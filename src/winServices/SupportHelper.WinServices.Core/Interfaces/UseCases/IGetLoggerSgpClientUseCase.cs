using SupportHelper.WinServices.Core.Models.Enums;

namespace SupportHelper.WinServices.Core.Interfaces.UseCases
{
    public interface IGetLoggerSgpClientUseCase
    {
        void Execute(SGPClientLine productionLine);
    }
}
