using System;
using System.Linq;
using System.Net;
using System.Threading;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Serilog;
using server.entities;
using server.helpers;
using server.networking;

namespace server
{
    public partial class ServerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private bool _shutdown;
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
            // Turns the unnecessary window small as it cannot be removed 
            _graphics.PreferredBackBufferHeight = 10;
            _graphics.PreferredBackBufferWidth = 10;
            _graphics.ApplyChanges();
            
            InitNetworkHandler();
            _playerManager = new PlayerManager();
            _command = new Thread(DoCommands);
            _command.Start();
            base.Initialize();
            Log.Information("Initialisation complete");
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
                var newPlayer = new ServerPlayerEntity(spawn, username, playerConnection);
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
    }
}