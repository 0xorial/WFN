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
        private MainWindowVm _mainWindowVm;
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
                    serviceInfo == null ? new string[0] : new[] {serviceInfo.Id},
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
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_mainWindowVm == null)
                {
                    var window = new MainWindow();
                    _mainWindowVm = new MainWindowVm(InitialRequest, () => window.Close());
                    window.DataContext = _mainWindowVm;
                    window.Closed += (sender, args) => { _mainWindowVm = null; };
                    window.Show();
                }
                else
                {
                    _mainWindowVm.AddNewRequest(requestInfo);
                }
            }));
        }
    }
}