using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models.Enums;

namespace SupportHelper.WinServices.Core.UseCases
{
    public sealed class GetLoggerSgpClientUseCase : IGetLoggerSgpClientUseCase
    {
        public void Execute(SGPClientLine productionLine)
        {
            string originBase = @$"C:\ProgramData\MBBras\SGP\SGPClient3";
            string folderName = Path.Combine(originBase, productionLine.ToString());
            string destinyBase = @"\\SERVIDOR\";
            string destiny = Path.Combine(destinyBase, $"Logs-{DateTime.Now:g}");

            VerifyFolderExists(folderName);
            string userLogIn = Environment.UserName;
            if (!userLogIn.StartsWith("D154_YSBC_"))
            {
                ConnectToSmb(destinyBase);
            }

            CopyFolder(folderName, destiny);
        }

        private static void VerifyFolderExists(string path)
        {
            if (!Directory.Exists(path))
                throw new Exception();
        }

        private bool ConnectToSmb(string serverDestin)
        {
            // LOGICAR PARA ACESSAR O SERVIDOR PASSANDO CREDENCIAIS
            throw new NotImplementedException();
        }

        private static void CopyFolder(string origin, string destiny)
        {
            if (Path.GetFullPath(destiny).StartsWith(Path.GetFullPath(origin), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("A pasta de destino não pode estar dentro da pasta de origem, senão causará recursão infinita.");

            if (!Directory.Exists(destiny))
                Directory.CreateDirectory(destiny);

            CopyFileInFolder(origin, destiny);

            foreach (string item in Directory.GetDirectories(origin))
            {
                string subFolderName = Path.GetFileName(item);
                string destinyFolder = Path.Combine(destiny, subFolderName);
                CopyFileInFolder(origin, destinyFolder);
            }
        }

        private static void CopyFileInFolder(string origin, string destiny)
        {
            if (!Directory.Exists(destiny))
                Directory.CreateDirectory(destiny);

            foreach (string item in Directory.GetFiles(origin))
            {
                string archiveName = Path.GetFileName(item);
                string archivedestiny = Path.Combine(destiny, archiveName);
                File.Copy(item, archivedestiny, true);
            }
        }
    }
}
