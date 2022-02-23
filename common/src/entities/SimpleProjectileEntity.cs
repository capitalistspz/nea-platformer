using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Serilog;

namespace common.entities
{
    public class SimpleProjectileEntity : ProjectileEntity
    {
        public Guid OwnerId;
        public int DamageAmount;

        public override IShapeF Bounds { get; }

        public SimpleProjectileEntity(Vector2 position, World world) : base(position, world)
        {
            Bounds = new CircleF(position, 1f);
        }

        public SimpleProjectileEntity(Vector2 position, World world, Guid guid) : base(position, world, guid)
        {
            Bounds = new CircleF(position, 1f);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is PlayerEntity player 
                && player.Id != OwnerId)
            {
                //player.Damage(this, DamageAmount);
                currentWorld.RemoveEntity(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, 4, Color.Red, thickness: 10);
        }

    }
}