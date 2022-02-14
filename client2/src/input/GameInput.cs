using System;
using System.Collections;
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
        private Dictionary<InputAction, TInputElement> _inputMap;

        public GameInput(ClientPlayerEntity owner)
        {
            _inputMap = new Dictionary<InputAction, TInputElement>();
            Owner = owner;
        }
        public abstract bool IsPressed(TInputElement key);

        public void AssignInput(InputAction action, TInputElement key)
        {
            _inputMap[action] = key;
        }
        public void AssignInput(IEnumerable<KeyValuePair<InputAction, TInputElement>> mapping)
        {
            _inputMap = new Dictionary<InputAction,TInputElement> (mapping);
        }
        
        public abstract Vector2 GetAimDirection();
        public virtual void Update()
        {
            var args = new InputEventArgs();
            if (IsPressed(_inputMap[InputAction.Jump]))
            {
                args.Actions[0] = true;
            }

            if (IsPressed(_inputMap[InputAction.Attack1]))
            {
                args.Actions[1] = true;
            }

            if (IsPressed(_inputMap[InputAction.Attack2]))
            {
                args.Actions[2] = true;
            }

            if (IsPressed(_inputMap[InputAction.Block]))
            {
                args.Actions[3] = true;
            }
            if (IsPressed(_inputMap[InputAction.Left]))
            {
                args.MovementDirection.X = -1f;
            }
            else if (IsPressed(_inputMap[InputAction.Right]))
            {
                args.MovementDirection.X = 1f;
            }
            if (IsPressed(_inputMap[InputAction.Down]))
            {
                args.MovementDirection.Y = 1f;
            }
            else if (IsPressed(_inputMap[InputAction.Up]))
            {
                args.MovementDirection.Y = -1f;
            }
            Owner.ApplyInput(args);
        }
        
    }

    
}