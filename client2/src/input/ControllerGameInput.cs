using client2.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace client2.input
{
    public class ControllerGameInput : GameInput<Buttons>
    {
        public readonly int ControllerId;
        public override bool IsPressed(Buttons button)
        {
            return GamePad.GetState(ControllerId).IsButtonDown(button);
        }
        
        public override Vector2 GetAimDirection()
        {
            return GamePad.GetState(ControllerId).ThumbSticks.Right;
        }

        public ControllerGameInput(ClientPlayerEntity owner) : base(owner)
        {
        }
    }
}