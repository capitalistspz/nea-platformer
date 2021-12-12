using System;
using common.entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace common.networking.S2C
{
    public class EntitySpawnedMessage : S2CMessage
    {
        private float _x;
        private float _y;
        private string _id;

        public EntitySpawnedMessage(BaseEntity entity)
        {
            (_x, _y) = entity.GetPosition();
            _id = entity.Id.ToString();
        }
        
        public EntitySpawnedMessage(NetIncomingMessage msg)
        {
            _ = msg.ReadByte();
            _id = msg.ReadString();
            _x = msg.ReadFloat();
            _y = msg.ReadFloat();

        }
        public static void ReadEntitySpawnedMessage(NetIncomingMessage msg, out Guid id, out Vector2 position)
        {
            _ = msg.ReadByte();
            id = Guid.Parse(msg.ReadString());
            position.X = msg.ReadFloat();
            position.Y = msg.ReadFloat();
        }
        public static void WriteEntityMotionMessage(NetOutgoingMessage msg, Guid id, Vector2 position)
        {
            msg.Write((byte)Type.EntityMotion);
            msg.Write(id.ToString());
            msg.Write(position.X);
            msg.Write(position.Y);
        }
        public override void SetData(NetOutgoingMessage msg)
        {
            msg.Write((byte)Type.EntitySpawned);
            msg.Write(_id);
            msg.Write(_x);
            msg.Write(_y);
        }
    }
}