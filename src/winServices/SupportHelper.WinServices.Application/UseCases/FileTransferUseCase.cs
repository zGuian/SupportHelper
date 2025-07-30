using EzSmb;
using EzSmb.Params;
using EzSmb.Params.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SupportHelper.WinServices.Application.Interfaces.UseCases;
using System.Diagnostics;

namespace SupportHelper.WinServices.Application.UseCases
{
    public class FileTransferUseCase : IFileTransferUseCase
    {
        private readonly string DomainName;
        private readonly string UserName;
        private readonly string Password;
        private readonly string IpServer;
        private readonly ILogger<FileTransferUseCase> _logger;

        public FileTransferUseCase(IConfiguration configuration, ILogger<FileTransferUseCase> logger)
        {
            _logger = logger;
            DomainName = configuration["SmbSettings:DomainName"] ?? throw new ArgumentNullException(DomainName, "NÃO ENCONTRADO VALOR DE APPSETTINGS");
            UserName = configuration["SmbSettings:Username"] ?? throw new ArgumentNullException(UserName, "NÃO ENCONTRADO VALOR DE APPSETTINGS");
            Password = configuration["SmbSettings:Password"] ?? throw new ArgumentNullException(Password, "NÃO ENCONTRADO VALOR DE APPSETTINGS");
            IpServer = configuration["SmbSettings:IpServer"] ?? throw new ArgumentNullException(IpServer, "NÃO ENCONTRADO VALOR DE APPSETTINGS");
        }

        public async Task<bool> SendArchiveZipAsync(string filePath, string archiveDestiny)
        {
            try
            {
                var sourceServer = await Node.GetNode(IpServer,
                    new ParamSet
                    {
                        DomainName = DomainName,
                        UserName = UserName,
                        Password = Password,
                        SmbType = SmbType.Smb2
                    });
                var archiveZipLocal = await sourceServer.GetNode(filePath);
                await archiveZipLocal.Move(archiveDestiny);
                ValidateFolder(archiveDestiny);
                sourceServer.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SendArchiveZipAsync((string filePath, string destinyArchive) tuple, CancellationToken cancellationToken = default)
        {
            MapperNetUse(tuple.destinyArchive);
            File.Copy(tuple.filePath, tuple.destinyArchive, overwrite: true);
            _logger.LogInformation("Arquivo copiado com sucesso.");
        }

        private void MapperNetUse(string serverDestiny)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = @$"net use \\{serverDestiny} \{Password} /user:{UserName}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var process = new Process
            {
                StartInfo = processInfo
            };
            process.Start();

            process.OutputDataReceived += (sender, args) =>
            {
                if (string.IsNullOrEmpty(args.Data))
                {

                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                process.BeginErrorReadLine();
            };
        }

        private void ValidateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            _logger.LogError("NÃO FOI ENCONTRADO CAMINHO {path}", path);
        }
    }
}
