using System.Collections.Generic;
using client2.entities;
using common.events;
using Microsoft.Xna.Framework;
using Serilog;

namespace client2.input
{
    public enum InputAction
    {
        Jump,
        Attack1,
        Attack2,
        Block,
        Left,
        Right,
        Up,
        Down,
    }

    public abstract class GameInput<TInputElement> : IGameInput
    {
        private long _lastUpdateTime;
        private long _minTimeDelay;
        public Dictionary<InputAction, TInputElement> InputMapping { get; private set; }
        
        public virtual Vector2 AimDirection { get; set; }
        public ClientPlayerEntity Owner { get; set; }
        public GameInput(ClientPlayerEntity owner)
        {
            InputMapping = new Dictionary<InputAction, TInputElement>();
            Owner = owner;
            _lastUpdateTime = 0;
            _minTimeDelay = 200000;
        }
        public abstract bool IsPressed(TInputElement key);

        public void AssignInput(InputAction action, TInputElement key)
        {
            InputMapping[action] = key;
        }
        public void AssignInput(IEnumerable<KeyValuePair<InputAction, TInputElement>> mapping)
        {
            InputMapping = new Dictionary<InputAction,TInputElement> (mapping);
        }
        
        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Ticks - _lastUpdateTime < _minTimeDelay)
                return;
            var args = new InputEventArgs();
            var updated = false;
            if (IsPressed(InputMapping[InputAction.Jump]))
            {
                args.Actions[0] = true;
                updated = true;
            }

            if (IsPressed(InputMapping[InputAction.Attack1]))
            {
                args.Actions[1] = true;
                updated = true;
            }

            if (IsPressed(InputMapping[InputAction.Attack2]))
            {
                args.Actions[2] = true;
                updated = true;
            }

            if (IsPressed(InputMapping[InputAction.Block]))
            {
                args.Actions[3] = true;
                updated = true;
            }
            if (IsPressed(InputMapping[InputAction.Left]))
            {
                args.MovementDirection.X = -1f;
                updated = true;
            }
            else if (IsPressed(InputMapping[InputAction.Right]))
            {
                args.MovementDirection.X = 1f;
                updated = true;
            }
            if (IsPressed(InputMapping[InputAction.Down]))
            {
                args.MovementDirection.Y = 1f;
                updated = true;
            }
            else if (IsPressed(InputMapping[InputAction.Up]))
            {
                args.MovementDirection.Y = -1f;
                updated = true;
            }
            args.AimDirection = AimDirection;
            if (updated)
            {
                Owner.ApplyInput(args);
                _lastUpdateTime = gameTime.TotalGameTime.Ticks;
            }
            
        }
        
    }

    
}