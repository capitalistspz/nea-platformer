using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace common.entities
{
    public class SimpleProjectileEntity : ProjectileEntity
    {
        private CircleF _bounds;
        public Guid OwnerId;
        public int DamageAmount;

        public override IShapeF Bounds => _bounds;

        public SimpleProjectileEntity(Vector2 position, World world) : base(position, world)
        {
            _bounds.Center = position;
            _bounds.Radius = 1f;
        }

        public SimpleProjectileEntity(Vector2 position, World world, Guid guid) : base(position, world, guid)
        {
            _bounds.Center = position;
            _bounds.Radius = 1f;
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is PlayerEntity player 
                && player.Id != OwnerId)
            {
                //player.Damage(this, DamageAmount);
                RemoveNextUpdate();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(_bounds, 1, Color.Red);
        }

    }
}