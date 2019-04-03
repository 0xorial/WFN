using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using Newtonsoft.Json;

namespace Notifier2
{
    public class IpcServer
    {
        public IpcServer()
        {
            WatchDirectory();
        }

        private void WatchDirectory()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    var combine = Path.Combine(Path.GetTempPath(), Common.IpcDirectoryName);
                    Directory.CreateDirectory(combine);
                    var files = Directory.GetFiles(combine, "*.json")
                        .ToArray();
                    // let writing finish
                    Thread.Sleep(100);
                    foreach (var file in files)
                    {
                        TryProcessFile(file);
                    }
                }
            }) {IsBackground = true};

            thread.Start();
        }

        private void TryProcessFile(string file)
        {
            string text;
            try
            {
                text = File.ReadAllText(file);
                File.Delete(file);
            }
            catch
            {
                Thread.Sleep(10);
                return;
            }

            var o = JsonConvert.DeserializeObject<ClientRequestInfo>(text);
            OnOnReceive(o);
        }

        public event Action<ClientRequestInfo> OnReceive;

        protected virtual void OnOnReceive(ClientRequestInfo obj)
        {
            OnReceive?.Invoke(obj);
        }
    }
}