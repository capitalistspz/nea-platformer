using System.Collections.Concurrent;
using System.Collections.Generic;
using common.networking;
using Lidgren.Network;
using MonoGame.Extended.Collections;
using Serilog;

namespace server.networking
{
    public partial class ServerNetHandler : NetHandler<NetServer>
    {
        private string _password;
        private Dictionary<NetConnection, string> _users;
        
        public ConcurrentQueue<NetConnection> RecentlyConnected;
        public ConcurrentQueue<NetConnection> RecentlyDisconnected;
        
        public ServerNetHandler(NetPeerConfiguration netServerConfig, string password)
        {
            _users = new Dictionary<NetConnection, string>();
            _NetPeer = new NetServer(netServerConfig);
            _Shutdown = false;
            _password = password;
            RecentlyConnected = new ConcurrentQueue<NetConnection>();
            RecentlyDisconnected = new ConcurrentQueue<NetConnection>();
        }

        public override void Shutdown()
        {
            RecentlyConnected.Clear();
            RecentlyDisconnected.Clear();
            _users.Clear();
            base.Shutdown();
        }
        
    }
}