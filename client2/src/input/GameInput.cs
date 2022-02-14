using System.Collections.Generic;
using client2.entities;
using common.events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

    public abstract class GameInput<TInputElement>
    {
        public readonly ClientPlayerEntity Owner;
        private Dictionary<InputAction, TInputElement> inputMap;

        public GameInput(ClientPlayerEntity owner)
        {
            Owner = owner;
        }
        public abstract bool IsPressed(TInputElement key);
        public abstract Vector2 GetAimDirection();
        public virtual void Update()
        {
            InputEventArgs args = new InputEventArgs();
            if (IsPressed(inputMap[InputAction.Jump]))
            {
                args.Actions[0] = true;
            }

            if (IsPressed(inputMap[InputAction.Attack1]))
            {
                args.Actions[1] = true;
            }

            if (IsPressed(inputMap[InputAction.Attack2]))
            {
                args.Actions[2] = true;
            }

            if (IsPressed(inputMap[InputAction.Block]))
            {
                args.Actions[3] = true;
            }
            if (IsPressed(inputMap[InputAction.Left]))
            {
                args.MovementDirection.X = -1f;
            }
            else if (IsPressed(inputMap[InputAction.Right]))
            {
                args.MovementDirection.X = 1f;
            }
            if (IsPressed(inputMap[InputAction.Down]))
            {
                args.MovementDirection.Y = 1f;
            }
            else if (IsPressed(inputMap[InputAction.Up]))
            {
                args.MovementDirection.Y = -1f;
            }
            Owner.ApplyInput(args);
        }
        
    }

    
}