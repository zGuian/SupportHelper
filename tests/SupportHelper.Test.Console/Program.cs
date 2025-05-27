using SupportHelper.Test.Console.Models;
using System.Text;

var machine = new MachineModel();
machine.GetAllInformationFromMachine();
var sb = new StringBuilder();
sb.AppendLine("ENCONTRADO OS SEGUINTES VALORES DO EQUIPAMENTO");
sb.AppendLine($"Dominio..... {machine.DomainName}");
sb.AppendLine($"Hostname do equipamento..... {machine.Hostname}");
sb.AppendLine($"Usuario atual..... {machine.CurrentUsername}");
sb.AppendLine($"Sistema Operacional..... {machine.OperationalSystem}");
foreach (var networkBoard in machine.NetworkBoards)
{
    sb.AppendLine("===========================================");
    sb.AppendLine("PROPRIEDADES DE REDE");
    sb.AppendLine($"Descrição..... {networkBoard.Description}");
    sb.AppendLine($"IPV4..... {networkBoard.Ipv4}");
    sb.AppendLine($"IPV6..... {networkBoard.Ipv6}");
    sb.AppendLine($"MacAddress..... {networkBoard.MacAddress}");
    sb.AppendLine($"Esta em uso..... {networkBoard.InUse}");
}
Console.WriteLine(sb.ToString());