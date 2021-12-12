using Lidgren.Network;

namespace common.networking.C2S
{
    public abstract class C2SMessage
    {
        public enum Type
        {
            Input
        }

        public abstract void SetData(NetOutgoingMessage msg);
    }
}