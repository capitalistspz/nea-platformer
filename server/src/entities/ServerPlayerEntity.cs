using System;
using common.entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace server.entities
{
    public class ServerPlayerEntity : PlayerEntity
    {
        public NetConnection Connection { get; }
        public ServerPlayerEntity(Vector2 position, string name, NetConnection connection) : base(position, name)
        {
            Connection = connection;
        }
    }
}