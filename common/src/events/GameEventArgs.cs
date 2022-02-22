using System;
using System.Collections;
using common.entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace common.events
{
    public class CreateEntityEventArgs : EventArgs
    {
        public Guid Id;
        public EntityType Type;
        public Vector2 SpawnPosition;
    }
    public class MovementEventArgs : EventArgs
    {
        public Guid Id;
        public Vector2 Position;
        public Vector2 Velocity;
    }
    public class DisconnectEventArgs : EventArgs
    {
        public NetConnection Connection;
    }
    public class ConnectEventArgs : EventArgs
    {
        public NetConnection Connection;
        public string Username;
    }
    public class InputEventArgs : EventArgs
    {
        public NetConnection Connection;
        public BitArray Actions = new(8);
        public Vector2 MovementDirection = Vector2.Zero;
        public Vector2 AimDirection = Vector2.Zero;
        public override string ToString()
        {
            return $"Actions: {Actions}, MovementDirection: {MovementDirection}, AimDirection: {AimDirection}";
        }
    }
}