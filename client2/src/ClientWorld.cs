using common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Serilog;

namespace client2
{
    public class ClientWorld : World
    {
        public ClientWorld(RectangleF worldBounds, TiledMap map, TiledMapRenderer mapRenderer) 
            : base(worldBounds, map, mapRenderer)
        {
            
        }
        public ClientWorld(RectangleF worldBounds) : base(worldBounds){}

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix mapTransform)
        {
            _mapRenderer?.Draw(mapTransform);
            foreach (var visibleEntity in _entityManager.FilterEntities(e => e.IsVisible()))
                visibleEntity.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, Matrix.Identity);
            base.Draw(gameTime, spriteBatch);
        }
        
    }
}