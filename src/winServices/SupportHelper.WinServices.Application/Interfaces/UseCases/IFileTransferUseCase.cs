namespace SupportHelper.WinServices.Application.Interfaces.UseCases
{
    public interface IFileTransferUseCase
    {
        Task<bool> SendArchiveZipAsync(string filePath, string archiveDestiny);
        void SendArchiveZipAsync((string filePath, string destinyArchive) tuple, CancellationToken cancellationToken = default);
    }
}
