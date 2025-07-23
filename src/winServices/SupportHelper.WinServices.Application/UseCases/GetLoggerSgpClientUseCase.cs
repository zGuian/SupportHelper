using Microsoft.Extensions.Logging;
using SupportHelper.WinServices.Application.Interfaces.UseCases;
using System.IO.Compression;
using System.Text;

namespace SupportHelper.WinServices.Application.UseCases
{
    public sealed class GetLoggerSgpClientUseCase : IGetLoggerSgpClientUseCase
    {
        private readonly ILogger<GetLoggerSgpClientUseCase> _logger;

        public GetLoggerSgpClientUseCase(ILogger<GetLoggerSgpClientUseCase> logger)
        {
            _logger = logger;
        }

        public string Execute(string productionLine)
        {
            var originBase = @$"C:\ProgramData\MBBras\SGP\SGPClient3";
            var folderName = Path.Combine(originBase, productionLine);
            if (!Directory.Exists(folderName))
            {
                _logger.LogInformation("NÃO FOI ENCONTRADO O CAMINHO {foldername}", folderName);
            }
            var destinyZipBase = @"C:\Temp\";
            if (!Directory.Exists(destinyZipBase))
            {
                Directory.CreateDirectory(destinyZipBase);
            }
            var origin = Path.Combine(folderName, "log");
            var destiny = Path.Combine(destinyZipBase, AppointedFolder());
            return ZipFolder(origin, destiny);
        }

        private static string AppointedFolder()
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy:HH:mm");
            string[] split = date.Split('/');
            string value = string.Join("", split);
            split = value.Split(':');
            value = string.Join("", split);
            var sb = new StringBuilder();
            sb.Append("logs_");
            sb.Append(value);
            return sb.ToString();
        }

        private static string ZipFolder(string origin, string destinyZip)
        {
            if (File.Exists(destinyZip))
            {
                File.Delete(destinyZip);
            }

            try
            {
                destinyZip = string.Concat(destinyZip, @".zip");
                ZipFile.CreateFromDirectory(origin, destinyZip, CompressionLevel.Fastest, true);
                return destinyZip;
            }
            catch (Exception)
            {

                throw;
            }
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
    }
}
