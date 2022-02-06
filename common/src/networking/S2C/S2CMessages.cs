using System;
using common.entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace common.networking.S2C
{
    public static class S2CMessages
    {
        public enum Type
        {
            EntityMotion,
            EntitySpawned
        }

        public static class MotionUpdate
        {
            public static void Deconstruct(NetIncomingMessage msg, out Guid id, out Vector2 position, out Vector2 velocity)
            {
                _ = msg.ReadByte();
                id = Guid.Parse(msg.ReadString());
                position.X = msg.ReadFloat();
                position.Y = msg.ReadFloat();
                velocity.X = msg.ReadFloat();
                velocity.Y = msg.ReadFloat();
            }
            public static void SetData(NetOutgoingMessage msg, Guid id, Vector2 position, Vector2 velocity)
            {
                msg.Write((byte)Type.EntitySpawned);
                msg.Write(id.ToString());
                msg.Write(position.X);
                msg.Write(position.Y);
                msg.Write(velocity.X);
                msg.Write(velocity.Y);
            }
        }
        public static class EntitySpawn
        {
            public static void Deconstruct(NetIncomingMessage msg, out Guid id, out Vector2 position, out EntityType entityType)
            {
                _ = msg.ReadByte();
                entityType = (EntityType)msg.ReadByte();
                id = Guid.Parse(msg.ReadString());
                position.X = msg.ReadFloat();
                position.Y = msg.ReadFloat();
            }
            public static void SetData(NetOutgoingMessage msg, Guid id, Vector2 position, EntityType entityType)
            {
                msg.Write((byte)Type.EntityMotion);
                msg.Write((byte)entityType);
                msg.Write(id.ToString());
                msg.Write(position.X);
                msg.Write(position.Y);
            }
        }
    }
}