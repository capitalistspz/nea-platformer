using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using common.networking.C2S;
using common.networking.S2C;
using Lidgren.Network;

namespace client
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var localAddress = "127.0.0.1";
            var port = 5632;
            Console.WriteLine($"Enter the server address (Default: {localAddress}): ");
            var inLocalAddress = Console.ReadLine();
            if (!string.IsNullOrEmpty(inLocalAddress))
                localAddress = inLocalAddress;
            Console.WriteLine($"Enter the port (Default: {port}): ");
            if (int.TryParse(Console.ReadLine(), out var inPort))
                port = inPort;
            Console.WriteLine("Enter your username: ");
            var username = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(username))
                username = "DEFAULT USERNAME";

            Console.WriteLine("Enter a server password (Leave empty if no password is needed): ");
            var password = Console.ReadLine() ?? string.Empty;
            ClientNetHandler handler = new ClientNetHandler();
            handler.Connect(localAddress, port, username, password);
        }
        
        
    }

    class ClientNetHandler
    {
        private Thread _cliNetThread;
        private NetClient _netClient;
        private bool _shutdown = false;
        public ClientNetHandler(){}
        public void Connect(string address, int port, string username, string serverPassword)
        {
            var cfg = new NetPeerConfiguration("ogame")
            {
                AutoFlushSendQueue = true
            };
            _netClient = new NetClient(cfg);
            
            // For username and password validation
            _netClient.Start();
            var approval = _netClient.CreateMessage();
            _netClient.Connect(address, port, approval);
            new ApprovalMessage(username, serverPassword).SetData(approval);
            _cliNetThread = new Thread(NetworkLoop);
            _cliNetThread.Start();
        }

        private void NetworkLoop()
        {
            while (!_shutdown)
            {
                var message = _netClient.ReadMessage();
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
                        Console.WriteLine($"[Waring] {message.ReadString()}");
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine($"[Error] {message.ReadString()}");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        HandleStatusChangedMessage(message);
                        break;
                    default:
                        Console.WriteLine($"[Warning] Unhandled message type: {message.MessageType}");
                        break;
                }
                _netClient.Recycle(message);
            }
            Console.WriteLine("[Networking] Shutdown");
        }

        private void HandleStatusChangedMessage(NetIncomingMessage message)
        {
            var msgType = (NetConnectionStatus)message.ReadByte();
            Console.WriteLine($"Message type: {msgType}");
            switch (msgType)
            {
                
            }
        }

        private void HandleDataMessage(NetIncomingMessage msg)
        {
            var type = (S2CMessage.Type) msg.PeekByte();
        }
    }
}