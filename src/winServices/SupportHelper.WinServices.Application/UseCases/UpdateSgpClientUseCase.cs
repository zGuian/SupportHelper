using SupportHelper.WinServices.Core.Interfaces.UseCases;
using System.Diagnostics;
using System.Text;

namespace SupportHelper.WinServices.Application.UseCases
{
    public sealed class UpdateSgpClientUseCase : IUpdateSgpClientUseCase
    {
        Process _process = new();

        public void Execute(string sgpClientLine)
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
    }
}
