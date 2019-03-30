using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using CommandLine;
using Newtonsoft.Json;

namespace Notifier2
{
    public static class EntryPoint
    {
        private static string _wfnNotifierPipe = "wfn_notifier_pipe";

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
            var parserResult = Parser.Default.ParseArguments(args);
            parserResult.WithParsed<Request>(options =>
            {
                using (PipeStream pipeClient = new NamedPipeClientStream(".", _wfnNotifierPipe))
                using (var writer = new BinaryWriter(pipeClient))
                {
                    var s = JsonConvert.ToString(options);
                    writer.Write(s);
                }
            });
           
        }

        private static void RunMainApp(string[] args)
        {
            if (args.Length == 0)
            {
                var app = new App {InitialRequest = new Request()};
                app.InitializeComponent();
                app.Run();
                return;
            }
            
            var thread = new Thread(() =>
            {
                using (var pipeServer = new NamedPipeServerStream(_wfnNotifierPipe, PipeDirection.In))
                using (var reader = new BinaryReader(pipeServer))
                {
                    var s = reader.ReadString();
                    var o = JsonConvert.DeserializeObject<Request>(s);
                    RequestsDispatcher.DispatchRequest(o);
                }
            }) {IsBackground = true};

            thread.Start();

            var parserResult = Parser.Default.ParseArguments(args);
            parserResult.WithParsed<Request>(options =>
            {
                var app = new App {InitialRequest = options};
                app.InitializeComponent();
                app.Run();
            });
        }
    }
}