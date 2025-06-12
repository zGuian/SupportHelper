using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models.Enums;
using System.Diagnostics;

namespace SupportHelper.WinServices.Core.UseCases
{
    public sealed class UpdateSgpClientUseCase : IUpdateSgpClientUseCase
    {
        Process _process = new();

        public void Execute(SGPClientLine sgpClientLine)
        {
            StopSgpIfRunning();
            var originPathBase = @"C:\ProgramData\MBBras\SGP";
            var origin = Path.Combine(originPathBase, sgpClientLine.ToString());
            if (Directory.Exists(origin))
            {
                var pathFile = Path.Combine(origin, "DCX.ITLC.SGPClient_Start.exe");
                _process.StartInfo.FileName = origin;
            }
        }

        private void StopSgpIfRunning()
        {
            var processes = Process.GetProcesses(Environment.MachineName);
            foreach (var item in processes)
            {
                if (item.ProcessName.StartsWith("DCX.ITLC"))
                {
                    item.Kill(true);
                    _process = item;
                    return;
                }
            }
        }
    }
}
