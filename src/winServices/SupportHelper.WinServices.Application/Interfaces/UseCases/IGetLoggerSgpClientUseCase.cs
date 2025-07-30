namespace SupportHelper.WinServices.Application.Interfaces.UseCases
{
    public interface IGetLoggerSgpClientUseCase
    {
        void Execute(string productionLine, out string filePath);
    }
}
