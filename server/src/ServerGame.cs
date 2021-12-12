using System;
using System.Net;
using System.Threading;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using server.entities;
using server.helpers;
using server.networking;

namespace server
{
    public class ServerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private ServerNetHandler _serverNetHandler;
        private PlayerManager _playerManager;
        private Thread _command;
        public ServerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 10;
            _graphics.PreferredBackBufferWidth = 10;
            _graphics.ApplyChanges();
            InitNetworkHandler();
            _playerManager = new PlayerManager();
            _command = new Thread(DoCommands);
            _command.Start();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_spriteBatch = new SpriteBatch(GraphicsDevice);

            //  use this.Content to load your game content here
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateConnections();
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

            var localAddress = CmdMsg.PrintAndInput("Enter the local address (Default: 127.0.0.1): ");
            if (!string.IsNullOrEmpty(localAddress))
                config.LocalAddress = IPAddress.Parse(localAddress);
            
            if (int.TryParse(CmdMsg.PrintAndInput("Enter the port (Default: 5632): "), out var port))
                config.Port = port;
            
            var password = CmdMsg.PrintAndInput("Enter a password (Leave empty if no password is needed): ") ?? string.Empty;

            _serverNetHandler = new ServerNetHandler(config, password);
        }

        private void UpdateConnections()
        {
            if (_serverNetHandler.RecentlyConnected.TryDequeue(out var playerConnection))
            {
                var spawn = Vector2.Zero;
                var newPlayer = new ServerPlayerEntity(spawn, (string)playerConnection.Tag, playerConnection);
                _playerManager.AddPlayer(newPlayer);
            };
            if (_serverNetHandler.RecentlyDisconnected.TryDequeue(out playerConnection))
            {
                _playerManager.RemovePlayer(playerConnection);
            }
        }

        private void DoCommands()
        {
            var input = string.Empty;
            while (input != "quit")
            {
                input = Console.ReadLine();
                switch (input)
                {
                    case "getplayers":
                        CommandGetPlayers();
                        break;
                    case "kickall":
                        CommandKickPlayers();
                        break;
                }

                input = String.Empty;
            }
            
        }

        private void CommandKickPlayers()
        {
            
        }

        private void CommandGetPlayers()
        {
            Console.WriteLine("Players: ");
            var players = _playerManager.GetPlayers(_ => true);
            foreach (var player in players)
            {
                Console.WriteLine($"{player.Connection}: {player.Name}");
            }
        }

        private void Quit()
        {
            _serverNetHandler.Shutdown();
        }
    }
}