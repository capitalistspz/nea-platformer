using System;
using System.Data;
using common.entities;
using common.physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Serilog;

namespace common
{
    public class World
    {
        protected EntityManager _entityManager;
        protected CollisionComponent _collisionComponent;
        protected TiledMap _map;
        protected TiledMapRenderer _mapRenderer;

        public World(RectangleF worldBounds, TiledMap map, TiledMapRenderer mapRenderer)
        {
            _map = map;
            _mapRenderer = mapRenderer;
            _collisionComponent = new CollisionComponent(worldBounds);
            _entityManager = new EntityManager();
        }

        public World(RectangleF worldBounds)
        {
            _collisionComponent = new CollisionComponent(worldBounds);
            _entityManager = new EntityManager();

        }

        public void AddEntity(BaseEntity entity)
        {
            _collisionComponent.Insert(entity);
            _entityManager.AddEntity(entity);
        }
        
        // For non entity collision objects
        public void AddCollisionObject(ICollisionActor actor)
        {
            _collisionComponent.Insert(actor);
        }
        
        // For non entity collision objects
        public void RemoveCollisionObject(ICollisionActor actor)
        {
            _collisionComponent.Remove(actor);
        }
        
        public void RemoveEntity(BaseEntity entity)
        {
            _collisionComponent.Remove(entity);
            _entityManager.RemoveEntity(entity);
        }

        public void Update(GameTime gameTime)
        {
            _entityManager.Update(gameTime);
            _collisionComponent.Update(gameTime);
            _mapRenderer?.Update(gameTime);
        }
        
        // Takes objects from the collision layer of a map in order to make them collidable
        public void GenerateCollisions()
        {
            var collisionLayer = _map.GetLayer<TiledMapObjectLayer>("collision");
            foreach (var collisionObject in collisionLayer.Objects)
            {
                var bounds = new RectangleF(collisionObject.Position, collisionObject.Size);
                AddCollisionObject(new BasicCollisionObject(bounds));
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
            
        }
        
    }
}