using System;
using System.Net;
using System.Threading;
using common;
using common.events;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Serilog;
using server.entities;
using server.helpers;
using server.networking;

namespace server
{
    public partial class ServerGame : BaseGame
    {
        private bool _shutdown;
        private ServerNetHandler _serverNetHandler;
        private PlayerManager _playerManager;
        private Thread _command;
        public ServerGame()
        {
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Turns the unnecessary window small as it cannot be removed 
            _graphics.PreferredBackBufferHeight = 10;
            _graphics.PreferredBackBufferWidth = 10;
            _graphics.ApplyChanges();
            
            SubscribeEvents();
            InitNetworkHandler();
            
            _playerManager = new PlayerManager();
            _command = new Thread(DoCommands);
            _command.Start();
            base.Initialize();
            Log.Information("Initialisation complete");
        }

        protected override void Update(GameTime gameTime)
        {
            GameEvents.InvokeEvents();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // No drawing here!
        }

        private void InitNetworkHandler()
        {
            var config = new NetPeerConfiguration("ogame") {Port = 5632, LocalAddress = new IPAddress(new byte[] {127, 0, 0, 1})};
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            //config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.AcceptIncomingConnections = true;
            var localAddress = CmdMsg.PrintAndInput("Enter the local address (Default: 127.0.0.1): ");
            if (!string.IsNullOrEmpty(localAddress))
                config.LocalAddress = IPAddress.Parse(localAddress);
            
            if (int.TryParse(CmdMsg.PrintAndInput("Enter the port (Default: 5632): "), out var port))
                config.Port = port;
            
            var password = CmdMsg.PrintAndInput("Enter a password (Leave empty if no password is needed): ") ?? string.Empty;

            _serverNetHandler = new ServerNetHandler(config, password);
            _serverNetHandler.Run();
        }

        private void UpdateConnections()
        {
            if (_serverNetHandler.RecentlyConnected.TryDequeue(out var playerConnection))
            {
                var spawn = Vector2.Zero;
                var username = (string)playerConnection.Tag;
                var newPlayer = new ServerPlayerEntity(spawn,  _world, username, playerConnection);
                _playerManager.AddPlayer(newPlayer);
            }

            if (_serverNetHandler.RecentlyDisconnected.TryDequeue(out playerConnection))
            {
                _playerManager.RemovePlayer(playerConnection);
            }
        }

        private void DoCommands()
        {
            Log.Information("Commands can now be entered.");
            while (!_shutdown)
            {
                var input = Console.ReadLine();

                switch (input)
                {
                    case "quit":
                        Quit();
                        break;
                    case "getplayers":
                        CommandGetPlayers();
                        break;
                    case "kick":
                        CommandKickPlayer();
                        break;
                    case "kickall":
                        CommandKickAllPlayers();
                        break;
                }
            }
        }
        

        private void Quit()
        {
            _shutdown = true;
            _serverNetHandler.Shutdown();
            Exit();
        }

        public void OnConnect(object sender, ConnectEventArgs args)
        {
            Log.Information("Player {@PlayerName} connected", args.Username);
            var spawn = Vector2.Zero;
            var newPlayer = new ServerPlayerEntity(spawn, _world, args.Username, args.Connection );
            _playerManager.AddPlayer(newPlayer);
        }

        public void OnDisconnect(object sender, DisconnectEventArgs args)
        {
            var username = _playerManager.GetPlayer(args.Connection).Name;
            Log.Information("Player {@PlayerName} disconnected", username);
            _playerManager.RemovePlayer(args.Connection);
        }

        public void SubscribeEvents()
        {
            GameEvents.Connect += OnConnect;
            GameEvents.Disconnect += OnDisconnect;
        }
    }
}