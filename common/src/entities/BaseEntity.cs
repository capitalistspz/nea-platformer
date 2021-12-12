using System;
using Microsoft.Xna.Framework;

namespace common.entities
{
    public abstract class BaseEntity
    {
        private Vector2 Position;
        private Vector2 Velocity;
        public readonly Guid Id;

        public BaseEntity(Vector2 position)
        {
            Velocity = Vector2.Zero;
            Id = Guid.NewGuid();
            Position = position;
        }
        public BaseEntity(Vector2 position, Guid guid)
        {
            Velocity = Vector2.Zero;
            Id = guid;
            Position = position;
        }

        public void SetPosition(Vector2 position) => Position = position;
        public Vector2 GetPosition() => Position;
        public void SetVelocity(Vector2 velocity) => Position = velocity;
        public Vector2 GetVelocity() => Velocity;
        public void AddVelocity(Vector2 deltaVelocity) => Velocity += deltaVelocity;
        
    }
}