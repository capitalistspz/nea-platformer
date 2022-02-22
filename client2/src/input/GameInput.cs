using System.Collections.Generic;
using client2.entities;
using common.events;
using Microsoft.Xna.Framework;

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
        public Dictionary<InputAction, TInputElement> InputMapping { get; private set; }

        public virtual Vector2 AimDirection { get; set; }
        public ClientPlayerEntity Owner { get; set; }
        public GameInput(ClientPlayerEntity owner)
        {
            InputMapping = new Dictionary<InputAction, TInputElement>();
            Owner = owner;
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
        
        public virtual void Update()
        {
            var args = new InputEventArgs();
            if (IsPressed(InputMapping[InputAction.Jump]))
            {
                args.Actions[0] = true;
            }

            if (IsPressed(InputMapping[InputAction.Attack1]))
            {
                args.Actions[1] = true;
            }

            if (IsPressed(InputMapping[InputAction.Attack2]))
            {
                args.Actions[2] = true;
            }

            if (IsPressed(InputMapping[InputAction.Block]))
            {
                args.Actions[3] = true;
            }
            if (IsPressed(InputMapping[InputAction.Left]))
            {
                args.MovementDirection.X = -1f;
            }
            else if (IsPressed(InputMapping[InputAction.Right]))
            {
                args.MovementDirection.X = 1f;
            }
            if (IsPressed(InputMapping[InputAction.Down]))
            {
                args.MovementDirection.Y = 1f;
            }
            else if (IsPressed(InputMapping[InputAction.Up]))
            {
                args.MovementDirection.Y = -1f;
            }
            Owner.ApplyInput(args);
        }
        
    }

    
}