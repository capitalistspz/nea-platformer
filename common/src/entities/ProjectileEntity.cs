using System;
using Microsoft.Xna.Framework;

namespace common.entities
{
    public class ProjectileEntity : BaseEntity
    {
        public ProjectileEntity(Vector2 position) : base(position)
        {
            SetVisibility(true);
        }

        public ProjectileEntity(Vector2 position, Guid guid) : base(position, guid)
        {
            SetVisibility(true);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}