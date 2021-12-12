using Lidgren.Network;

namespace common.networking.S2C
{
    public abstract class S2CMessage
    {
        public enum Type
        {
            EntityMotion,
            EntitySpawned
        }
        public abstract void SetData(NetOutgoingMessage msg);
    }
}