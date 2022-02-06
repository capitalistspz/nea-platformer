using System.Threading;
using Lidgren.Network;
using Serilog;

namespace common.networking
{
    public abstract class NetHandler<T> where T : NetPeer
    {
        private Thread _loopThread;
        protected T _NetPeer;
        protected bool _Shutdown;

        protected NetHandler()
        {
            _loopThread = new Thread(NetworkLoop) {Name = "Net handler"};
        }

        public void Run()
        {
            _Shutdown = false;
            _NetPeer.Start();
            _loopThread.Start();
            
        }

        public virtual void Shutdown()
        {
            _Shutdown = true;
            _NetPeer.Shutdown("Shutdown.");
            _loopThread.Join();
        }

        protected void NetworkLoop()
        {
            while (!_Shutdown)
            {
                var message = _NetPeer.WaitMessage(-1);
                Log.Debug("Message Received: {@MessageType}", message.MessageType);
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        HandleDataMessage(message);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        Log.Verbose("{@VerboseMessage}", message.ReadString());
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Log.Debug("{@DebugMessage}", message.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Log.Warning("{@WarningMessage}", message.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Log.Error("{@ErrorMessage}", message.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        HandleStatusChange(message);
                        break;
                    case NetIncomingMessageType.Error:
                        Log.Error("!! {@Error}", message.ReadString());
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        HandleApproval(message);
                        break;
                }
            }
        }

        protected abstract void HandleDataMessage(NetIncomingMessage message);
        protected abstract void HandleStatusChange(NetIncomingMessage message);
        protected abstract void HandleApproval(NetIncomingMessage message);
    }
}