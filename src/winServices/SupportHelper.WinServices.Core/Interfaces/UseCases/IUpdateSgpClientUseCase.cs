using SupportHelper.WinServices.Core.Models.Enums;

namespace SupportHelper.WinServices.Core.Interfaces.UseCases
{
    public interface IUpdateSgpClientUseCase
    {
        void Execute(string sgpClientLine);
    }
}
