using System.Diagnostics.CodeAnalysis;
using Lidgren.Network;

namespace common.networking.C2S
{
    public static class C2SMessages
    {
        public enum Type
        {
            Input,
            Approval
        }
        
        public static class Approval
        {
            public static void SetData(ref NetOutgoingMessage outMsg, string username, string serverPassword)
            {
                outMsg.Write((byte)Type.Approval);
                outMsg.Write(username);
                outMsg.Write(serverPassword);
            }
            public static void Deconstruct(NetIncomingMessage incMsg, out string username,
                out string serverPassword)
            {
                _ = incMsg.ReadByte();
                username = incMsg.ReadString();
                serverPassword = incMsg.ReadString();
            }
        }
        

        
    }
}