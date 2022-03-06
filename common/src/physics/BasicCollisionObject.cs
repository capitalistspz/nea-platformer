using System;
using common.entities;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Serilog;

namespace common.physics
{
    public class BasicCollisionObject : ICollisionActor
    {
        public IShapeF Bounds { get; }

        public BasicCollisionObject(RectangleF collisionBounds)
        {
            Bounds = collisionBounds;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is BaseEntity entity)
            {
                entity.SetPosition(entity.GetPosition() + collisionInfo.PenetrationVector);
                Log.Debug("Collided with entity");
            }
        }

    }
}