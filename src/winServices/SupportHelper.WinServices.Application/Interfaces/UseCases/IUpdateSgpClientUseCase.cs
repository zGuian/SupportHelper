using SupportHelper.WinServices.Core.Models.Enums;

namespace SupportHelper.WinServices.Application.Interfaces.UseCases
{
    public interface IUpdateSgpClientUseCase
    {
        void Execute(string sgpClientLine);
    }
}
