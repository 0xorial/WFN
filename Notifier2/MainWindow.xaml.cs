using System.Collections.Generic;
using System.Windows;

namespace Notifier2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Queue<Request> RequestQueue { get; set; }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ProcessNextRequest();
        }

        private void ProcessNextRequest()
        {
            var request = RequestQueue.Dequeue();
            var vm = new MainWindowVM(request, NextRequest) {TotalEvents = RequestQueue.Count};
            DataContext = vm;
        }

        private void NextRequest()
        {
            if (RequestQueue.Count == 0)
            {
                Close();
            }

            ProcessNextRequest();
        }
    }
}