using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using common.networking.C2S;
using common.networking.S2C;
using Lidgren.Network;
using MonoGame.Extended.Collections;

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
            _shutdown = false;
            _password = password;
            RecentlyConnected = new ConcurrentQueue<NetConnection>();
            RecentlyDisconnected = new ConcurrentQueue<NetConnection>();
        }

        public void Run()
        {
            _netServer.Start();
            _loopThread = new Thread(NetworkLoop);
            _loopThread.Start();
        }

        public void Shutdown()
        {
            _shutdown = true;
            _netServer.Shutdown("Server shutdown.");
            _loopThread.Join();
        }
        private void NetworkLoop()
        {
            Console.WriteLine("[Networking] Started");
            while (!_shutdown)
            {
                var message = _netServer.ReadMessage();
                if (message == null)
                    continue;
                Console.WriteLine($"Message Received: {message.MessageType}");
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
            Console.WriteLine("[Networking] Shutdown");
        }

        private void HandleDataMessage(NetIncomingMessage message)
        {
            var msgType = (C2SMessage.Type) message.PeekByte();
            switch (msgType)
            {
                case C2SMessage.Type.Input:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void HandleApproval(NetIncomingMessage message)
        {
            var approval = new ApprovalMessage(message);
            if (_password != approval.Password)
                message.SenderConnection.Deny("Incorrect server password.");
            else if (_usernames.Contains(approval.Username))
                message.SenderConnection.Deny("There is already a user in the server with that name.");
            else
                message.SenderConnection.Approve();
        }
        private void HandleStatusChange(NetIncomingMessage message)
        {
            var status = (NetConnectionStatus) message.ReadByte();
            switch (status)
            {
                case NetConnectionStatus.InitiatedConnect:
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    break;
                case NetConnectionStatus.RespondedConnect:
                    break;
                case NetConnectionStatus.Connected:
                    if (_approved.Remove(message.SenderConnection.RemoteUniqueIdentifier, out string username))
                    {
                        _usernames.Add(username);
                        message.SenderConnection.Tag = username;
                        RecentlyConnected.Enqueue(message.SenderConnection);
                    }
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    RecentlyDisconnected.Enqueue(message.SenderConnection);
                    break;
                case NetConnectionStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}