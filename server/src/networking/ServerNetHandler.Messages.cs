using System;
using common.networking.C2S;
using Lidgren.Network;
using Serilog;

namespace server.networking
{
    public partial class ServerNetHandler
    {
        private void HandleDataMessage(NetIncomingMessage message)
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
        private void HandleApproval(NetIncomingMessage message)
        {
            C2SMessages.Approval.Deconstruct(message, out var username, out var password);
            string denyReason = null;
            if (_password != password)
                denyReason = "Incorrect server password.: " + password;
            else if (string.IsNullOrWhiteSpace(username) || _usernames.Contains(username))
                denyReason = "Invalid username.";
            
            if (denyReason != null)
            {
                message.SenderConnection.Deny(denyReason);
                return;
            }
            message.SenderConnection.Approve();
            _usernames.Add(username);
            message.SenderConnection.Tag = username;
            RecentlyConnected.Enqueue(message.SenderConnection);
        }
        private void HandleStatusChange(NetIncomingMessage message)
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
                    Log.Information("Player connected.");
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    Log.Information("Player disconnected.");
                    RecentlyDisconnected.Enqueue(message.SenderConnection);
                    _usernames.Remove((string) message.SenderConnection.Tag);
                    break;
                case NetConnectionStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), " Unknown net connection status.");
            }
        }
    }
}