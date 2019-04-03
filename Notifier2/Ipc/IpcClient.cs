using System;
using System.IO;
using System.IO.Pipes;
using Newtonsoft.Json;

namespace Notifier2
{
    public class IpcClient
    {
        public void NotifyServer(ClientRequestInfo requestInfo)
        {
            var s = JsonConvert.SerializeObject(requestInfo);
            File.WriteAllText(
                Path.Combine(Path.GetTempPath(), Common.IpcDirectoryName, new Guid().ToString() + ".json"),
                s);
        }
    }
}