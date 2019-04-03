using System;
using System.Linq;
using System.Windows;
using Wokhan.WindowsFirewallNotifier.Common.Helpers;

namespace Notifier2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public ClientRequestInfo InitialRequest { get; set; }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            new IpcServer().OnReceive += info =>
            {
                var serviceInfo = ServiceResolver.TryGetServiceName(info);

                // Check whether this connection is blocked by a rule.
                var blockingRules = FirewallHelper.GetMatchingRules(info.Path,
                    ProcessHelper.getAppPkgId(info.Pid),
                    info.ProtocolIanaId,
                    info.TargetIp,
                    info.TargetPort.ToString(),
                    info.LocalPort.ToString(),
                    new[] {serviceInfo.Id},
                    ProcessHelper.getLocalUserOwner(info.Pid),
                    true);
                if (blockingRules.Any())
                {
                    return;
                }

                HandleRequest(info);
            };
            
            HandleRequest(InitialRequest);
        }

        private void HandleRequest(ClientRequestInfo requestInfo)
        {
            MainWindowVm vm = null;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (vm == null)
                {
                    var window = new MainWindow();
                    vm = new MainWindowVm(InitialRequest, () => window.Close());
                    window.DataContext = vm;
                    window.Show();
                }
                else
                {
                    vm.AddNewRequest(requestInfo);
                }
            }));
        }
    }
}