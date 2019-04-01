using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Notifier2.Annotations;

namespace Notifier2
{
    public sealed class MainWindowVm : INotifyPropertyChanged
    {        
        public MainWindowVm(Request request, Action onDone)
        {
            _request = request;
            var ruleBuilder = new RuleBuilder(this);
            AllowCommand = new RelayCommand(() =>
            {
                ruleBuilder.Allow = true;
                RuleCreator.CreateRule(ruleBuilder);
                onDone();
            });
            BlockCommand = new RelayCommand(() =>
            {
                ruleBuilder.Allow = false;
                RuleCreator.CreateRule(ruleBuilder);
                onDone();
            });
            SkipCommand = new RelayCommand(onDone);
        }

        private int _totalEvents;

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

        private Request _request;

        public Request Request
        {
            get => _request;
            set
            {
                if (_request == value) return;
                _request = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand AllowCommand { get; set; }
        public ICommand BlockCommand { get; set; }
        public ICommand SkipCommand { get; set; }

        private bool _useProtocol;

        public bool UseProtocol
        {
            get => _useProtocol;
            set
            {
                if (_useProtocol == value) return;
                _useProtocol = value;
                OnPropertyChanged();
            }
        }

        private bool _useLocalPort;

        public bool UseLocalPort
        {
            get => _useLocalPort;
            set
            {
                if (_useLocalPort == value) return;
                _useLocalPort = value;
                OnPropertyChanged();
            }
        }

        private bool _useService;

        public bool UseService
        {
            get => _useService;
            set
            {
                if (_useService == value) return;
                _useService = value;
                OnPropertyChanged();
            }
        }

        private bool _useTargetPort;

        public bool UseTargetPort
        {
            get => _useTargetPort;
            set
            {
                if (_useTargetPort == value) return;
                _useTargetPort = value;
                OnPropertyChanged();
            }
        }

        private bool _useTargetIp;

        public bool UseTargetIp
        {
            get => _useTargetIp;
            set
            {
                if (_useTargetIp == value) return;
                _useTargetIp = value;
                OnPropertyChanged();
            }
        }

        private bool _useAppId;

        public bool UseAppId
        {
            get => _useAppId;
            set
            {
                if (_useAppId == value) return;
                _useAppId = value;
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