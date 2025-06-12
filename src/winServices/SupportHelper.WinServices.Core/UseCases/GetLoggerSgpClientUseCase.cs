using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models.Enums;
using System.IO;
using System.Text;

namespace SupportHelper.WinServices.Core.UseCases
{
    public sealed class GetLoggerSgpClientUseCase : IGetLoggerSgpClientUseCase
    {
        public void Execute(SGPClientLine productionLine)
        {
            string originBase = @$"C:\ProgramData\MBBras\SGP\SGPClient3";
            string folderName = Path.Combine(originBase, productionLine.ToString());
            string destinyBase = @"\\SERVIDOR\";
            string destiny = Path.Combine(destinyBase, AppointedFolder());

            if (!Directory.Exists(folderName)) 
                throw new Exception();

            string userLogIn = Environment.UserName;
            if (!userLogIn.StartsWith("D154_YSBC_"))
            {
                ConnectToSmb(destinyBase);
            }

            CopyFolder(folderName, destiny);
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
