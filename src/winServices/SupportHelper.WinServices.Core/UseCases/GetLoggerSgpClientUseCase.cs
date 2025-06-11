using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models.Enums;

namespace SupportHelper.WinServices.Core.UseCases
{
    public sealed class GetLoggerSgpClientUseCase : IGetLoggerSgpClientUseCase
    {
        public Task ExecuteAsync(SGPClientLine productionLine, CancellationToken cancellationToken = default)
        {
            string originBase = @$"C:\ProgramData\MBBras\SGP\SGPClient3";
            string folderName = Path.Combine(originBase, productionLine.ToString());
            string destinyBase = @"\\SERVIDOR\";
            string destiny = Path.Combine(destinyBase, $"Logs-{DateTime.Now:g}");

            VerifyFolderExists(folderName);
            var userLogIn = Environment.UserName;
            if (!userLogIn.StartsWith("D154_YSBC_"))
            {
                ConnectSmb(destinyBase);
            }

            CopyFolder(folderName, destiny);

            throw new NotImplementedException();
        }

        private void VerifyFolderExists(string path)
        {
            if (!Directory.Exists(path))
                throw new Exception();
        }

        private bool ConnectSmb(string serverDestin)
        {
            // LOGICAR PARA ACESSAR O SERVIDOR PASSANDO CREDENCIAIS
            throw new NotImplementedException();
        }

        private void CopyFolder(string origin,string destiny)
        {
            Directory.CreateDirectory(destiny);

            foreach (var item in Directory.GetFiles(origin))
            {
                string archiveName = Path.GetFileName(item);
                File.Copy(item, destiny, true);
            }
        }
    }
}
