using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using SupportHelper.WinServices.Application.Interfaces.UseCases;
using System.Threading.Channels;

namespace SupportHelper.WinServices.Application.UseCases
{
    public class FileTransferUseCase : IFileTransferUseCase
    {
        private readonly string _hubUrl;

        public FileTransferUseCase(IConfiguration configuration)
        {
            _hubUrl = $"{configuration["SignalrSettings:FileTransferUrl"]}?hostname={Environment.MachineName}";
        }

        public async Task SendArchiveZipAsync(string productionLine, string filePath, CancellationToken cancellationToken = default)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();

            await connection.StartAsync(cancellationToken);

            var fileName = Path.GetFileName(filePath);
            var channel = Channel.CreateUnbounded<byte[]>();

            var sendTask = connection.SendAsync("ReceberArquivoZip", channel.Reader, fileName, cancellationToken);

            using var stream = File.OpenRead(filePath);
            var buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await channel.Writer.WriteAsync(buffer[..bytesRead], cancellationToken);
            }

            channel.Writer.Complete();
            await sendTask;

            await connection.DisposeAsync();
        }
    }
}
