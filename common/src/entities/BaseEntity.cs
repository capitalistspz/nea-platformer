using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace common.entities
{
    public abstract class BaseEntity
    {
        private Vector2 _position;
        private Vector2 _velocity;
        
        // Whether the entity is rendered or not
        private bool _visible;
        
        // Unique identifier for each entity
        public readonly Guid Id;

        public BaseEntity(Vector2 position)
        {
            _velocity = Vector2.Zero;
            Id = Guid.NewGuid();
            _position = position;
            _visible = false;
        }
        public BaseEntity(Vector2 position, Guid guid)
        {
            _velocity = Vector2.Zero;
            Id = guid;
            _position = position;
            _visible = true;
        }
        // Physics, inputs etc
        public abstract void Update(GameTime gameTime);
        
        // Drawing stuff
        public virtual void Draw(SpriteBatch spriteBatch){}
        
        // Obvious
        public void SetPosition(Vector2 position) => _position = position;
        public Vector2 GetPosition() => _position;
        public void SetVelocity(Vector2 velocity) => _velocity = velocity;
        public Vector2 GetVelocity() => _velocity;
        public void AddVelocity(Vector2 deltaVelocity) => _velocity += deltaVelocity;
        public void SetVisibility(bool shouldBeVisible) => _visible = shouldBeVisible;
        public bool IsVisible() => _visible;

    }
}