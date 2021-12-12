using System;
using common.entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace common.networking.S2C
{
    public class EntityMotionUpdateMessage : S2CMessage
    {
        private readonly float _x;
        private readonly float _y;
        private readonly float _velX;
        private readonly float _velY;
        private string _id;
        public EntityMotionUpdateMessage(Guid id, Vector2 position, Vector2 velocity)
        {
            _id = id.ToString();
            (_x, _y) = position;
            (_velX, _velY) = velocity;
        }
        public EntityMotionUpdateMessage(NetIncomingMessage msg)
        {
            _id = msg.ReadString();
            _x = msg.ReadFloat();
            _y = msg.ReadFloat();
            _velX = msg.ReadFloat();
            _velY = msg.ReadFloat();
        }
        public override void SetData(NetOutgoingMessage msg)
        {
            msg.Write((byte)Type.EntityMotion);
            msg.Write(_id);
            msg.Write(_x);
            msg.Write(_y);
            msg.Write(_velX);
            msg.Write(_velY);
        }
        public static void ReadEntityMotionMessage(NetIncomingMessage msg, out Guid id, out Vector2 position, out Vector2 velocity)
        {
            _ = msg.ReadByte();
            id = Guid.Parse(msg.ReadString());
            position.X = msg.ReadFloat();
            position.Y = msg.ReadFloat();
            velocity.X = msg.ReadFloat();
            velocity.Y = msg.ReadFloat();
        }

        public static void WriteEntityMotionMessage(NetOutgoingMessage msg, Guid id, Vector2 position, Vector2 velocity)
        {
            msg.Write((byte)Type.EntityMotion);
            msg.Write(id.ToString());
            msg.Write(position.X);
            msg.Write(position.Y);
            msg.Write(velocity.X);
            msg.Write(velocity.Y);
        }
        public Vector2 Position => new(_x, _y);
        public Vector2 Velocity => new(_velX, _velY);
        public Guid Guid => Guid.Parse(_id);
    }
}