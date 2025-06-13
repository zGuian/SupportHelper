using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models.Enums;
using System.Diagnostics;
using System.Text;

namespace SupportHelper.WinServices.Core.UseCases
{
    public sealed class UpdateSgpClientUseCase : IUpdateSgpClientUseCase
    {
        Process _process = new();

        public void Execute(SGPClientLine sgpClientLine)
        {
            Process[] processes = Process.GetProcesses(Environment.MachineName);

            string originPathBase = @"C:\ProgramData\MBBras\SGP";
            string? origin = Path.Combine(originPathBase, sgpClientLine.ToString());

            if (!Directory.Exists(origin))
            {
                SearchSgpByProcessesOpen(processes, sgpClientLine, ref origin);
            }

            StopSgpIfRunning(processes);

            string pathFile = Path.Combine(origin, "DCX.ITLC.SGPClient_Start.exe");
            _process.StartInfo.FileName = pathFile;
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

        private static void SearchSgpByProcessesOpen(Process[] processes, SGPClientLine sgpCLientLine, ref string pathDirectory)
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
                    if (item == sgpCLientLine.ToString())
                    {
                        pathDirectory = sb.ToString();
                        break;
                    }
                }
            }
            throw new Exception("SGP NÃO ESTA RODANDO NA MÁQUINA");
        }
    }
}
