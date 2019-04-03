using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Threading;
using CommandLine;
using Newtonsoft.Json;
using Wokhan.WindowsFirewallNotifier.Common.Helpers;

namespace Notifier2
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var mutex = new Mutex(true, "start_mutex", out var createdNew);
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                RunMainApp(args);
                mutex.ReleaseMutex();
            }
            else
            {
                StartClientApp(args);
            }
        }

        private static void StartClientApp(string[] args)
        {
            var r = ParseArgs(args);
            new IpcClient().NotifyServer(r);
        }

        private static void RunMainApp(string[] args)
        {
            if (args.Length == 0)
            {
                var app = new App
                {
                    InitialRequest = new ClientRequestInfo(41, 1, "someip", 8080, 17, 2324, @"C:\somepath")
                };
                app.InitializeComponent();
                app.Run();
                return;
            }

            var r = ParseArgs(args);
            var app2 = new App
            {
                InitialRequest = r
            };
            app2.InitializeComponent();
            app2.Run();
        }

        private static ClientRequestInfo ParseArgs(string[] args)
        {
            var pars = ProcessHelper.ParseParameters(args);
            var pid = int.Parse(pars["pid"]);
            var threadid = int.Parse(pars["threadid"]);
            string currentTarget = pars["ip"];
            var currentTargetPort = int.Parse(pars["port"]);
            var currentProtocol = int.Parse(pars["protocol"]);
            var currentLocalPort = int.Parse(pars["localport"]);
            string currentPath = pars["path"];
            return new ClientRequestInfo(pid,
                threadid,
                currentTarget,
                currentTargetPort,
                currentProtocol,
                currentLocalPort,
                currentPath);
        }
    }
}