namespace Notifier2
{
    public class ClientRequestInfo
    {
        public int Pid { get; }
        public int ThreadId { get; }
        public string TargetIp { get; }
        public int TargetPort { get; }
        public int ProtocolIanaId { get; }
        public int LocalPort { get; }
        public string Path { get; }

        public ClientRequestInfo(int pid, int threadId, string targetIp, int targetPort, int protocolIanaId, int localPort, string path)
        {
            Pid = pid;
            ThreadId = threadId;
            TargetIp = targetIp;
            TargetPort = targetPort;
            ProtocolIanaId = protocolIanaId;
            LocalPort = localPort;
            Path = path;
        }
    }
}