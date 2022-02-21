using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace common.entities
{
    public abstract class BaseEntity : ICollisionActor
    {
        private Vector2 _velocity;
        
        // Whether the entity is rendered or not
        private bool _visible;
        private bool _removed;
        
        // Unique identifier for each entity
        public readonly Guid Id;
        
        public BaseEntity(Vector2 position)
        {
            
            _velocity = Vector2.Zero;
            _visible = false;
            _removed = false;
            Id = Guid.NewGuid();

        }
        public BaseEntity(Vector2 position, Guid guid)
        {
            _velocity = Vector2.Zero;
            _visible = true;
            _removed = false;
            Id = guid;

        }
        // Physics, inputs etc
        public abstract void Update(GameTime gameTime);
        
        // Drawing stuff
        public virtual void Draw(SpriteBatch spriteBatch){}
        // Marks entity to be destroyed
        public void RemoveNextUpdate() => _removed = true;
        // Obvious
        public void SetPosition(Vector2 position) => Bounds.Position = position;
        public Vector2 GetPosition() => Bounds.Position;
        public void SetVelocity(Vector2 velocity) => _velocity = velocity;
        public Vector2 GetVelocity() => _velocity;
        public void AddVelocity(Vector2 deltaVelocity) => _velocity += deltaVelocity;
        public void SetVisibility(bool shouldBeVisible) => _visible = shouldBeVisible;
        public bool IsVisible() => _visible;

        public abstract void OnCollision(CollisionEventArgs collisionInfo);
        public abstract IShapeF Bounds { get; }
    }
}