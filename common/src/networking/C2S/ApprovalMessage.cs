using Lidgren.Network;

namespace common.networking.C2S
{
    public class ApprovalMessage
    {
        private string _username;
        private string _serverPassword;

        public ApprovalMessage(string username, string serverPassword)
        {
            _username = username;
            _serverPassword = serverPassword;
        }

        public ApprovalMessage(NetIncomingMessage incMessage)
        {
            _username = incMessage.ReadString();
            _serverPassword = incMessage.ReadString();
        }

        public void SetData(NetOutgoingMessage outMessage)
        {
            outMessage.Write(_username);
            outMessage.Write(_serverPassword);
        }
        public string Username => _username;
        public string Password => _serverPassword;
    }
}