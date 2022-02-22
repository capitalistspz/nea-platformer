using System;
using Microsoft.Xna.Framework;

namespace common.entities
{
    public abstract class ProjectileEntity : BaseEntity
    {
        public ProjectileEntity(Vector2 position, World world) : base(position,  world)
        {
            SetVisibility(true);
        }

        public ProjectileEntity(Vector2 position, World world, Guid guid) : base(position, world, guid)
        {
            SetVisibility(true);
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float) (gameTime.ElapsedGameTime.TotalMilliseconds / 10);
            var position = GetPosition() + GetVelocity() * deltaTime;

            SetPosition(position);
        }
    }
}