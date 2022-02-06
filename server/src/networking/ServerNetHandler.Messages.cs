using System;
using common.events;
using common.networking.C2S;
using Lidgren.Network;
using Serilog;

namespace server.networking
{
    public partial class ServerNetHandler
    {
        protected override void HandleDataMessage(NetIncomingMessage message)
        {
            var msgType = (C2SMessages.Type) message.PeekByte();
            switch (msgType)
            {
                case C2SMessages.Type.Input:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void HandleApproval(NetIncomingMessage message)
        {
            C2SMessages.Approval.Deconstruct(message, out var username, out var password);
            string denyReason = null;
            if (_password != password)
                denyReason = "Incorrect server password.";
            else if (string.IsNullOrWhiteSpace(username) || _users.ContainsValue(username))
                denyReason = "Invalid username.";
            
            if (denyReason != null)
            {
                message.SenderConnection.Deny(denyReason);
                return;
            }
            message.SenderConnection.Approve();
            _users[message.SenderConnection] = username;
            GameEvents.EnqueueEvent(new ConnectEventArgs{Connection = message.SenderConnection, Username = username});
            //message.SenderConnection.Tag = username;
            //RecentlyConnected.Enqueue(message.SenderConnection);
        }

        protected override void HandleStatusChange(NetIncomingMessage message)
        {
            var status = (NetConnectionStatus) message.ReadByte();
            Log.Debug("Received Status Update: {@Status}", status);
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
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    GameEvents.EnqueueEvent(new DisconnectEventArgs{Connection = message.SenderConnection});
                    _users.Remove(message.SenderConnection);
                    break;
                case NetConnectionStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), " Unknown net connection status.");
            }
        }
    }
}