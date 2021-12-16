using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using common.networking.C2S;
using common.networking.S2C;
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
        private List<string> _usernames;
        // RemoteUniqueIdentifiers and Usernames
        private Dictionary<long, string> _approved;
        public ConcurrentQueue<NetConnection> RecentlyConnected;
        public ConcurrentQueue<NetConnection> RecentlyDisconnected;
        
        public ServerNetHandler(NetPeerConfiguration netServerConfig, string password)
        {
            _usernames = new List<string>();
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
            _shutdown = true;
            _netServer.Shutdown("Server shutdown.");
            _loopThread.Join();
            Log.Information("Message read loop shutdown.");
        }
        private void NetworkLoop()
        {
            while (!_shutdown)
            {
                var message = _netServer.ReadMessage();
                if (message == null)
                    continue;
                Log.Information($"[Debug] Message Received: {message.MessageType}");
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        HandleDataMessage(message);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine($"[Debug] {message.ReadString()}");
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine($"[Warn] {message.ReadString()}");
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine($"[Error] {message.ReadString()}");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        HandleStatusChange(message);
                        break;
                    case NetIncomingMessageType.Error:
                        Console.WriteLine($"[Error] {message.ReadString()}");
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        HandleApproval(message);
                        break;
                }
            }
            Log.Information("[Networking] Shutdown");
        }

        
        

    }
}