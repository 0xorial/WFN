using System.Linq;
using System.Windows.Input;
using Vanara.PInvoke;
using Wokhan.WindowsFirewallNotifier.Common.Helpers;

namespace Notifier2
{
    public class ServiceResolver
    {
        public class ServiceInfo
        {
            public string Id { get; set; }
            public string Description { get; set; }
        }
        public static ServiceInfo TryGetServiceName(ClientRequestInfo clientRequestInfo)
        {
            ProcessHelper.GetService(clientRequestInfo.Pid,
                clientRequestInfo.ThreadId,
                clientRequestInfo.Path,
                clientRequestInfo.ProtocolIanaId,
                clientRequestInfo.LocalPort,
                clientRequestInfo.TargetIp,
                clientRequestInfo.TargetPort,
                out var svc,
                out var svcdsc,
                out _);


            if (svc.Length != 0)
            {
                return new ServiceInfo
                {
                    Id = svc[0],
                    Description = svcdsc[0]
                };
            }

            return null;
        }
    }
}