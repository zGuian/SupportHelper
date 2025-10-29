
namespace SupportHelper.Service.Domain.Interface.UseCases
{
    public interface IGetLoggerSgpClientUseCase
    {
        void Execute(string productionLine, out string filePath);
        Task<bool> ExecuteAsync(string productionLine, string requestId);
    }
}
