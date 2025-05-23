using System.Net;
using System.Threading.Tasks;

namespace SupportHelper.Infrastructure.HttpServices
{
    public class HttpListenService
    {
        public HttpListenService() { }

        public async Task HttpListenerMachineInformation()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost/TESTE");
            listener.Start();

            while(true)
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;
                if (request.HasEntityBody)
                {
                    request.
                }
            }
        }
    }
}
