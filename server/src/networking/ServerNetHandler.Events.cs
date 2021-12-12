using System;
using System.Collections;
using common.entities;
using common.events;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace server.networking
{
    public partial class ServerNetHandler
    {
        public event EventHandler<CreateEntityEventArgs> CreateEntity;
        public event EventHandler<MovementEventArgs> Movement;
        public event EventHandler<ConnectEventArgs> Connect;
        public event EventHandler<DisconnectEventArgs> Disconnect;
        public event EventHandler<InputEventArgs> Input;
        
    }

    
}