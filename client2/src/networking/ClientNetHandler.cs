using System;
using common.entities;
using common.events;
using common.networking;
using common.networking.S2C;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace client2.networking
{
    public class ClientNetHandler : NetHandler<NetClient>
    {
        public ClientNetHandler(NetPeerConfiguration netClientConfig)
        {
            _NetPeer = new NetClient(netClientConfig);
        }

        protected override void HandleDataMessage(NetIncomingMessage message)
        {
            var msgType = (S2CMessages.Type) message.PeekByte();
            switch (msgType)
            {
                case S2CMessages.Type.EntityMotion:
                    S2CMessages.MotionUpdate.Deconstruct(message, out var id, out var position, out var velocity);
                    var movementArgs = new MovementEventArgs {Id = id, Position = position, Velocity = velocity};
                    GameEvents.EnqueueEvent(movementArgs);
                    break;
                case S2CMessages.Type.EntitySpawned:
                    S2CMessages.EntitySpawn.Deconstruct(message, out var newEntityId, out var spawnPosition, out var entityType);
                    var spawnArgs = new CreateEntityEventArgs {Id = newEntityId, SpawnPosition = spawnPosition, Type = entityType};
                    GameEvents.EnqueueEvent(spawnArgs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void HandleStatusChange(NetIncomingMessage message)
        {
            var status = (NetConnectionStatus) message.MessageType;
            switch (status)
            {
                
            }
            throw new NotImplementedException();
        }

        protected override void HandleApproval(NetIncomingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}