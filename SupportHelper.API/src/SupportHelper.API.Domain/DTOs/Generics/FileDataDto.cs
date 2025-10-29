namespace SupportHelper.API.Domain.DTOs.Generics
{
    public class FileDataDto : IDisposable
    {
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public Stream Stream { get; set; } = default!;
        public bool HasData { get; set; }

        public FileDataDto()
        {
            HasData = false;
        }

        public FileDataDto(string fileName, string contentType, Stream stream)
        {
            FileName = fileName;
            ContentType = contentType;
            Stream = stream;
            HasData = true;
        }

        public void Dispose()
        {
            Stream?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
