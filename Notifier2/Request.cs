namespace Notifier2
{
    public class Request
    {
        public int Pid { get; set; }
        public int ThreadId { get; set; }
        public string TargetIp { get; set; }
        public int TargetPort { get; set; }
        public string Protocol { get; set; }
        public int ProtocolIanaId { get; set; }
        public int LocalPort { get; set; }
        public string Path { get; set; }

        public bool HasService => ServiceName != null;
        public string ServiceName { get; set; }

        public bool HasAppId => AppId != null;
        public string AppId { get; set; }
    }
}