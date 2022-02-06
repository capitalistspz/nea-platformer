using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;

namespace common.events
{
    public static class GameEvents
    {
        public static event EventHandler<CreateEntityEventArgs> CreateEntity;
        public static event EventHandler<MovementEventArgs> Movement;
        public static event EventHandler<ConnectEventArgs> Connect;
        public static event EventHandler<DisconnectEventArgs> Disconnect;
        public static event EventHandler<InputEventArgs> Input;

        private static ConcurrentQueue<CreateEntityEventArgs> CreateEntityEventArgsQueue = new();
        private static ConcurrentQueue<MovementEventArgs> MovementEventArgsQueue = new();
        private static ConcurrentQueue<ConnectEventArgs> ConnectEventArgsQueue = new();
        private static ConcurrentQueue<DisconnectEventArgs> DisconnectEventArgsQueue = new();
        private static ConcurrentQueue<InputEventArgs> InputEventArgsQueue = new();
        public static void InvokeCreateEntity()
        {
            while (!CreateEntityEventArgsQueue.IsEmpty)
            {
                CreateEntityEventArgsQueue.TryDequeue(out var args);
                if (args != null)
                    CreateEntity?.Invoke(null, args);
            }
        }
        
        public static void InvokeConnect()
        {
            while (!ConnectEventArgsQueue.IsEmpty)
            {
                ConnectEventArgsQueue.TryDequeue(out var args);
                if (args != null)
                    Connect?.Invoke(null, args);
            }
        }
        public static void InvokeMovement()
        {
            while (!MovementEventArgsQueue.IsEmpty)
            {
                MovementEventArgsQueue.TryDequeue(out var args);
                if (args != null)
                    Movement?.Invoke(null, args);
            }
        }

        public static void InvokeDisconnect()
        {
            while (!DisconnectEventArgsQueue.IsEmpty)
            {
                DisconnectEventArgsQueue.TryDequeue(out var args);
                if (args != null)
                    Disconnect?.Invoke(null, args);
            }
        }

        public static void InvokeInput()
        {
            while (!InputEventArgsQueue.IsEmpty)
            {
                InputEventArgsQueue.TryDequeue(out var args);
                if (args != null)
                    Input?.Invoke(null, args);
            }
        }

        public enum EventType
        {
            CreateEntity,
            Movement,
            Input,
            Connect,
            Disconnect
        }

        public static void EnqueueEvent(EventArgs args)
        {
            switch (args)
            {
                case InputEventArgs eventArgs:
                    InputEventArgsQueue.Enqueue(eventArgs);
                    break;
                case ConnectEventArgs eventArgs:
                    ConnectEventArgsQueue.Enqueue(eventArgs);
                    break;
                case DisconnectEventArgs eventArgs:
                    DisconnectEventArgsQueue.Enqueue(eventArgs);
                    break;
                case MovementEventArgs eventArgs:
                    MovementEventArgsQueue.Enqueue(eventArgs);
                    break;
                case CreateEntityEventArgs eventArgs:
                    CreateEntityEventArgsQueue.Enqueue(eventArgs);
                    break;
            }
        }
        
        // Invokes all events
        public static void InvokeEvents()
        {
            InvokeConnect();
            InvokeDisconnect();
            InvokeCreateEntity();
            InvokeMovement();
            InvokeInput();
        }
    }
}