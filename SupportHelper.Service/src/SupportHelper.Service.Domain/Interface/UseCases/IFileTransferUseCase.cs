namespace SupportHelper.Service.Domain.Interface.UseCases
{
    public interface IFileTransferUseCase
    {
        Task<bool> SendArchiveZipAsync(string filePath, string archiveDestiny);
        void SendArchiveZipAsync((string filePath, string destinyArchive) tuple, CancellationToken cancellationToken = default);
    }
}
