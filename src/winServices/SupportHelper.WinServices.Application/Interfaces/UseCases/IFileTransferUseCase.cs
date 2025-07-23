namespace SupportHelper.WinServices.Application.Interfaces.UseCases
{
    public interface IFileTransferUseCase
    {
        Task SendArchiveZipAsync(string productionLine, string filePath, CancellationToken cancellationToken = default);
    }
}
