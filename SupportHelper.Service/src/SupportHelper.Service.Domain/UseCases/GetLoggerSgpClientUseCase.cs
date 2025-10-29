using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Interface.UseCases;
using System.IO.Compression;
using System.Net.Http.Headers;

namespace SupportHelper.Service.Domain.UseCases
{
    public sealed class GetLoggerSgpClientUseCase : IGetLoggerSgpClientUseCase
    {
        private readonly ILogger<GetLoggerSgpClientUseCase> _logger;
        private readonly IHttpClientFactory _httpClient;

        public GetLoggerSgpClientUseCase(ILogger<GetLoggerSgpClientUseCase> logger
            , IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<bool> ExecuteAsync(string productionLine, string requestId)
        {
            var originBase = @$"C:\ProgramData\MBBras\SGP\SGPClient3";
            var folderName = Path.Combine(originBase, productionLine);
            if (!Directory.Exists(folderName))
            {
                _logger.LogInformation("NÃO FOI ENCONTRADO O CAMINHO {foldername}", folderName);
                return false;
            }
            var destinyZipBase = @"C:\Temp\";
            if (!Directory.Exists(destinyZipBase))
            {
                Directory.CreateDirectory(destinyZipBase);
            }
            var origin = Path.Combine(folderName, "log");
            var destiny = Path.Combine(destinyZipBase, AppointedFolder());
            var fileZip = ZipFolder(origin, destiny);
            return await SendFileInHttpAsync(fileZip, requestId);
        }

        public void Execute(string productionLine, out string filePath)
        {
            var originBase = @$"C:\ProgramData\MBBras\SGP\SGPClient3";
            var folderName = Path.Combine(originBase, productionLine);
            if (!Directory.Exists(folderName))
            {
                _logger.LogInformation("NÃO FOI ENCONTRADO O CAMINHO {foldername}", folderName);
                filePath = "ERROR";
                return;
            }
            var destinyZipBase = @"C:\Temp\";
            if (!Directory.Exists(destinyZipBase))
            {
                Directory.CreateDirectory(destinyZipBase);
            }
            var origin = Path.Combine(folderName, "log");
            var destiny = Path.Combine(destinyZipBase, AppointedFolder());
            filePath = ZipFolder(origin, destiny);
        }

        private static string AppointedFolder()
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
            string[] split = date.Split('/');
            string value = string.Join("", split);
            split = value.Split(':');
            value = string.Join("", split);
            return $"logs_{value}";
        }

        private static string ZipFolder(string origin, string destinyZip)
        {
            if (File.Exists(destinyZip))
            {
                File.Delete(destinyZip);
            }
            destinyZip = string.Concat(destinyZip, ".zip");
            ZipFile.CreateFromDirectory(origin, destinyZip, CompressionLevel.Optimal, true);
            return destinyZip;
        }

        private static void CopyFolderAndArchives(string origin, string destiny)
        {
            if (Path.GetFullPath(destiny).StartsWith(Path.GetFullPath(origin), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("A pasta de destino não pode estar dentro da pasta de origem, senão causará recursão infinita.");

            if (!Directory.Exists(destiny))
                Directory.CreateDirectory(destiny);

            foreach (var item in Directory.GetFiles(origin))
            {
                string archiveName = Path.GetFileName(item);
                string archivedestiny = Path.Combine(destiny, archiveName);
                File.Copy(item, archivedestiny, true);
            }

            foreach (var item in Directory.GetDirectories(origin))
            {
                string subFolderName = Path.GetFileName(item);
                string destinyFolder = Path.Combine(destiny, subFolderName);
                CopyFolderAndArchives(item, destinyFolder);
            }
        }

        private async Task<bool> SendFileInHttpAsync(string filePath, string receivedId)
        {
            using var client = _httpClient.CreateClient("APISupportHelper");
            using var fileStream = File.OpenRead(filePath);
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(fileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
            content.Headers.Add("Id-Request", receivedId);
            content.Add(fileContent, "file", Path.GetFileName(filePath));
            var response = await client.PostAsync("api/v1/ReceivedFiles", content);
            return response.IsSuccessStatusCode;
        }
    }
}
