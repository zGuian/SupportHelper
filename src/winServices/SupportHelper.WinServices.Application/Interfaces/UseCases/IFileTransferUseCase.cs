namespace SupportHelper.WinServices.Application.Interfaces.UseCases
{
    public interface IFileTransferUseCase
    {
        Task SendArchiveZipAsync(string filePath, string archiveDestiny);
        void SendArchiveZipAsync((string filePath, string destinyArchive) tuple, CancellationToken cancellationToken = default);
    }
}
