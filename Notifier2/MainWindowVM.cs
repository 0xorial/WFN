using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Notifier2.Annotations;

namespace Notifier2
{
    public sealed class MainWindowVM : INotifyPropertyChanged
    {
        private int _port;
        private int _totalEvents;

        public MainWindowVM(Request request, Action onDone)
        {
        }

        public int Port
        {
            get => _port;
            set
            {
                if (value == _port) return;
                _port = value;
                OnPropertyChanged();
            }
        }

        public int TotalEvents
        {
            get => _totalEvents;
            set
            {
                if (value == _totalEvents) return;
                _totalEvents = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}