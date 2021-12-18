using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using common.networking.C2S;
using common.networking.S2C;
using Lidgren.Network;
using Serilog;

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
            Log.Logger = Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo
                .File("logs/client.log", rollingInterval: RollingInterval.Hour).CreateLogger();
            
            Console.WriteLine("Enter a server password (Leave empty if no password is needed): ");
            var password = Console.ReadLine() ?? string.Empty;
            ClientNetHandler handler = new ClientNetHandler();
            handler.Connect(localAddress, port, username, password);
        }
        
        
    }

    class ClientNetHandler
    {
        private Thread _loopThread;
        private NetClient _netClient;
        private bool _shutdown;

        public ClientNetHandler()
        {
            _shutdown = false;
        }
        public void Connect(string address, int port, string username, string serverPassword)
        {
            var cfg = new NetPeerConfiguration("ogame") { AutoFlushSendQueue = true };
            _netClient = new NetClient(cfg);
            // For username and password validation
            _netClient.Start();
            _loopThread = new Thread(NetworkLoop);
            _loopThread.Start();
            var approval = _netClient.CreateMessage();
            C2SMessages.Approval.SetData(ref approval, username, serverPassword);
            _netClient.Connect(address, port , approval);
        }

        private void NetworkLoop()
        {
            while (!_shutdown)
            {
                var message = _netClient.WaitMessage(-1);
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
                        HandleStatusChangedMessage(message);
                        break;
                    default:
                        Log.Warning("Unhandled message type: {@MessageType}", message.MessageType);
                        break;
                }
                _netClient.Recycle(message);
            }
            Log.Information("Networking shutdown.");
        }

        private void HandleStatusChangedMessage(NetIncomingMessage message)
        {
            var status = (NetConnectionStatus)message.ReadByte();
            Log.Debug("Message type: {@Status}", status);
            Log.Information(message.ReadString());
        }

        private void HandleDataMessage(NetIncomingMessage msg)
        {
            var type = (S2CMessage.Type) msg.PeekByte();
        }
    }
}