using System;
using System.Windows;

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
            new IpcServer().OnReceive += HandleRequest;
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