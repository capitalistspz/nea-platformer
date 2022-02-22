using System;
using common;
using common.entities;
using Microsoft.Xna.Framework;

namespace client2.entities
{
    public class ClientPlayerEntity : PlayerEntity
    {
        public ClientPlayerEntity(Vector2 position, World world, string name) : base(position, world, name)
        {
            
        }
    }
}