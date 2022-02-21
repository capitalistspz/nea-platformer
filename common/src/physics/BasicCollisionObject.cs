using MonoGame.Extended;
using MonoGame.Extended.Collisions;

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
            
        }

    }
}