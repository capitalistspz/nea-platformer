using System;
using System.Collections.Generic;
using System.IO;
using common.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Serilog;

namespace common
{
    public class BaseGame : Game
    {
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        protected World _world;

        private List<TiledMap> _tiledMaps;
        public BaseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _tiledMaps = new List<TiledMap>();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }
        
        // Finds all tiled map files in content folder and then loads them into memory
        public void LoadMaps()
        {
            var place = new DirectoryInfo(Content.RootDirectory + @"/tiled/map");
            foreach (var mapsFileName in place.GetFiles())
            {
                try
                {
                    var name = mapsFileName.Name;
                    var map = Content.Load<TiledMap>(name[..name.LastIndexOf("xnb", StringComparison.Ordinal)]);
                    _tiledMaps.Add(map);
                }
                catch (Exception ignored)
                {
                    // ignored
                }
            }
        }
        
        protected void AddEntity(BaseEntity entity)
        {
            _world.AddEntity(entity);
        }

        protected void RemoveEntity(BaseEntity entity)
        {
            _world.AddEntity(entity);
        }
        
        
    }
}