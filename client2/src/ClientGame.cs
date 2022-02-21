using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using client2.entities;
using client2.input;
using common;
using common.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Myra;
using Myra.Graphics2D.UI;
using Serilog;


namespace client2
{
    public class ClientGame : BaseGame
    {
        private List<IGameInput> _inputs;
        private List<ClientPlayerEntity> _localPlayers;
        private Desktop _desktop;

        public ClientGame()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _world = new ClientWorld(new RectangleF(0, 0, 1920, 1080));
            _inputs = new List<IGameInput>();
            _localPlayers = new List<ClientPlayerEntity>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Adds local players to the world
            foreach (var player in _localPlayers)
                AddEntity(player);
            
            LoadTextures("textures");

            // For Myra GUI
            MyraEnvironment.Game = this;
            _desktop = new Desktop();
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            ConstructGUI();
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            foreach (var input in _inputs)
                input.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            // Draw from here
            _world.Draw(gameTime, _spriteBatch);
            // To here 
            _spriteBatch.End();
            _desktop.Render();
            base.Draw(gameTime);
            
        }

        protected void LoadTextures(string textureRoot)
        {
            var playerTexture = Content.Load<Texture2D>(textureRoot + "/entities/player");
            PlayerEntity.PlayerTexture = playerTexture;
        }

        protected void ConstructGUI()
        {
            var data = File.ReadAllText("ui/default.xml");
            var project = Project.LoadFromXml(data);
            _desktop.Widgets.Add(project.Root);
        }
        
        
    }
}