namespace SupportHelper.WinServices.Application.Interfaces.UseCases
{
    public interface IFileTransferUseCase
    {
        Task SendArchiveZipAsync(string productionLine, string filePath, string requestId, CancellationToken cancellationToken = default);
    }
}
