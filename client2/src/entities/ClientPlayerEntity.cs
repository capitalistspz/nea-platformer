using System;
using common.entities;
using Microsoft.Xna.Framework;

namespace client2.entities
{
    public class ClientPlayerEntity : PlayerEntity
    {
        public ClientPlayerEntity(Vector2 position, string name) : base(position, name)
        {
            
        }

        public ClientPlayerEntity(Vector2 position, Guid guid) : base(position, guid)
        {
            
        }

        public string GetCurrentSprite()
        {
            return "textures/entities/player";
        }
    }
}