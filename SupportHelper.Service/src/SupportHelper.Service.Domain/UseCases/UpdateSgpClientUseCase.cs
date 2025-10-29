using SupportHelper.Service.Domain.Interface.UseCases;
using System.Diagnostics;
using System.Text;

namespace SupportHelper.Service.Domain.UseCases
{
    public sealed class UpdateSgpClientUseCase : IUpdateSgpClientUseCase
    {
        Process _process = new();

        public string Execute(string sgpClientLine)
        {
            Process[] processes = Process.GetProcesses(Environment.MachineName);

            string originPathBase = @"C:\ProgramData\MBBras\SGP";
            string? origin = Path.Combine(originPathBase, sgpClientLine);
            //origin = C:\ProgramData\MBBras\SGP\SGPClient3_TR

            if (!Directory.Exists(origin))
            {
                SearchSgpByProcessesOpen(processes, sgpClientLine, ref origin);
            }

            StopSgpIfRunning(processes);

            string pathFile = Path.Combine(origin, "DCX.ITLC.SGPClient_Start.exe");
            //pathFile = C:\ProgramData\MBBras\SGP\SGPClient3_TR\DCX.ITLC.SGPClient_Start.exe

            _process.StartInfo.FileName = pathFile;
            _process.Start();
            var (version, isSuccess) = GetSgpVersion(pathFile);
            if (!isSuccess)
            {
                return version;
            }
            return $"Versão atual: {version}";
        }

        private void StopSgpIfRunning(Process[] processes)
        {
            foreach (Process item in processes)
            {
                if (item.ProcessName.StartsWith("DCX.ITLC"))
                {
                    item.CloseMainWindow();
                    _process = item;
                }

                if (!_process.HasExited)
                    _process.Kill();
            }
        }

        private static void SearchSgpByProcessesOpen(Process[] processes, string sgpCLientLine, ref string pathDirectory)
        {
            foreach (Process process in processes)
            {
                if (!process.ProcessName.StartsWith("DCX.ITLC"))
                {
                    continue;
                }

                string path = process.MainModule.FileName;
                string[] array = path.Split('\\');
                StringBuilder sb = new();
                foreach (string item in array)
                {
                    sb.Append(Path.Combine("\\", item));
                    if (item == sgpCLientLine)
                    {
                        pathDirectory = sb.ToString();
                        break;
                    }
                }
            }
            throw new Exception("SGP NÃO ESTA RODANDO NA MÁQUINA");
        }

        private static (string version, bool isSuccess) GetSgpVersion(string pathFile)
        {
            if (!File.Exists(pathFile))
            {
                return ("não foi encontrado aplicação", false);
            }
            var infoVersion = FileVersionInfo.GetVersionInfo(pathFile);
            if (infoVersion.FileVersion is null)
                return ("não encontrado versão da aplicação", false);
            return (infoVersion.FileVersion, true);
        }
    }
}
