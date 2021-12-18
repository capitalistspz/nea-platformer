using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Lidgren.Network;
using MonoGame.Extended.Collections;
using Serilog;

namespace server.networking
{
    public partial class ServerNetHandler
    {
        private Thread _loopThread;
        private NetServer _netServer;
        private bool _shutdown;
        private string _password;
        private Bag<string> _usernames;
        // RemoteUniqueIdentifiers and Usernames
        private Dictionary<long, string> _approved;
        public ConcurrentQueue<NetConnection> RecentlyConnected;
        public ConcurrentQueue<NetConnection> RecentlyDisconnected;
        
        public ServerNetHandler(NetPeerConfiguration netServerConfig, string password)
        {
            _usernames = new Bag<string>();
            _approved = new Dictionary<long, string>();
            _netServer = new NetServer(netServerConfig);
            _loopThread = new Thread(NetworkLoop) { Name = "Message Read Loop" };
            _shutdown = false;
            _password = password;
            RecentlyConnected = new ConcurrentQueue<NetConnection>();
            RecentlyDisconnected = new ConcurrentQueue<NetConnection>();
        }

        public void Run()
        {
            _netServer.Start();
            _loopThread.Start();
        }

        public void Shutdown()
        {
            _netServer.Shutdown("Server shutdown.");
            _shutdown = true;
            _loopThread.Join();
            Log.Debug("Message read loop shutdown.");
        }
        private void NetworkLoop()
        {
            while (!_shutdown)
            {
                var message = _netServer.ReadMessage();
                if (message == null)
                    continue;
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
                        Log.Error("|| {@Error}", message.ReadString());
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        HandleApproval(message);
                        break;
                }
            }
        }
    }
}