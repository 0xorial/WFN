using System;
using System.Collections.Generic;
using System.Windows;

namespace Notifier2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private MainWindow _window;
        private readonly Queue<Request> _queue = new Queue<Request>();
        public Request InitialRequest { get; set; }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _queue.Enqueue(InitialRequest);
            ProcessRequest(InitialRequest);

            RequestsDispatcher.DispatchRequest = request => Dispatcher.BeginInvoke(new Action(() =>
            {
                ProcessRequest(request);
            }));
        }

        private void ProcessRequest(Request request)
        {
            if (_window == null)
            {
                _window = new MainWindow {RequestQueue = _queue};
                _window.Show();
            }
            else
            {
                _queue.Enqueue(request);
            }
        }
    }
}