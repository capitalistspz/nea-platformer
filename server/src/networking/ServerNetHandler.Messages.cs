using System;
using common.networking.C2S;
using Lidgren.Network;

namespace server.networking
{
    public partial class ServerNetHandler
    {
        private void HandleDataMessage(NetIncomingMessage message)
        {
            var msgType = (C2SMessage.Type) message.PeekByte();
            switch (msgType)
            {
                case C2SMessage.Type.Input:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void HandleApproval(NetIncomingMessage message)
        {
            var approval = new ApprovalMessage(message);
            if (_password != approval.Password)
                message.SenderConnection.Deny("Incorrect server password.");
            else if (_usernames.Contains(approval.Username))
                message.SenderConnection.Deny("There is already a user in the server with that name.");
            else
                message.SenderConnection.Approve();
        }
        private void HandleStatusChange(NetIncomingMessage message)
        {
            var status = (NetConnectionStatus) message.ReadByte();
            switch (status)
            {
                case NetConnectionStatus.InitiatedConnect:
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    break;
                case NetConnectionStatus.RespondedConnect:
                    break;
                case NetConnectionStatus.Connected:
                    if (_approved.Remove(message.SenderConnection.RemoteUniqueIdentifier, out string username))
                    {
                        _usernames.Add(username);
                        message.SenderConnection.Tag = username;
                        RecentlyConnected.Enqueue(message.SenderConnection);
                    }
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    RecentlyDisconnected.Enqueue(message.SenderConnection);
                    break;
                case NetConnectionStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}