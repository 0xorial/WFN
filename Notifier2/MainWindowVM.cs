using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Notifier2.Annotations;
using Wokhan.WindowsFirewallNotifier.Common.Helpers;

namespace Notifier2
{
    public sealed class MainWindowVm : INotifyPropertyChanged
    {
        private readonly Action _closeWindow;
        private readonly Queue<ClientRequestInfo> _queue = new Queue<ClientRequestInfo>();


        public MainWindowVm(ClientRequestInfo ri, Action closeWindow)
        {
            _closeWindow = closeWindow;
            _request = MakeRequest(ri);
            var ruleBuilder = new RuleBuilder(this);
            AllowAllCommand = new RelayCommand(() =>
            {
                ruleBuilder.Allow = true;
                RuleCreator.CreateRule(ruleBuilder);
                OnDone();
            });
            BlockAllCommand = new RelayCommand(() =>
            {
                ruleBuilder.Allow = false;
                RuleCreator.CreateRule(ruleBuilder);
                OnDone();
            });
            SkipCommand = new RelayCommand(OnDone);
        }

        private Request MakeRequest(ClientRequestInfo ri)
        {
            return new Request
            {
                Pid = ri.Pid,
                Path = ri.Path,
                ProtocolIanaId = ri.ProtocolIanaId,
                Protocol = FirewallHelper.getProtocolAsString(ri.ProtocolIanaId),
                TargetIp = ri.TargetIp,
                ThreadId = ri.ThreadId,
                LocalPort = ri.LocalPort,
                TargetPort = ri.TargetPort,
                AppId = ProcessHelper.getAppPkgId(ri.Pid),
                ServiceName = ServiceResolver.TryGetServiceName(ri)?.Description
            };
        }

        private void OnDone()
        {
            if (_queue.Count == 0)
            {
                _closeWindow();
            }
            else
            {
                Request = MakeRequest(_queue.Dequeue());
                TotalEvents = _queue.Count;
            }
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

        public ICommand AllowAllCommand { get; set; }
        public ICommand BlockAllCommand { get; set; }
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

        public void AddNewRequest(ClientRequestInfo request)
        {
            _queue.Enqueue(request);
            TotalEvents = _queue.Count;
        }
    }
}