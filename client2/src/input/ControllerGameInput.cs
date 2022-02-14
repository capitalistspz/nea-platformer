using System;
using System.Collections.Generic;
using client2.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace client2.input
{
    public class ControllerGameInput : GameInput<Buttons>
    {
        private readonly int _controllerId;
        public override bool IsPressed(Buttons button)
        {
            return GamePad.GetState(_controllerId).IsButtonDown(button);
        }
        
        public override Vector2 GetAimDirection()
        {
            return GamePad.GetState(_controllerId).ThumbSticks.Right;
        }

        public ControllerGameInput(ClientPlayerEntity owner, int controllerId) : base(owner)
        {
            AssignInput(DefaultMapping);
            if (controllerId > 0)
                _controllerId = controllerId;
            else
                throw new ArgumentOutOfRangeException(nameof(controllerId));
        }

        public Dictionary<InputAction, Buttons> DefaultMapping => new()
        {

            [InputAction.Jump] = Buttons.A,
            [InputAction.Attack1] = Buttons.B,
            [InputAction.Attack2] = Buttons.Y,
            [InputAction.Block] = Buttons.X,
            [InputAction.Left] = Buttons.DPadLeft,
            [InputAction.Right] = Buttons.DPadRight,
            [InputAction.Up] = Buttons.DPadUp,
            [InputAction.Down] = Buttons.DPadDown
        };
        
    }
}