using client2.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace client2.input
{
    public class KeyboardGameInput : GameInput<Keys>
    {
        public override bool IsPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public override Vector2 GetAimDirection()
        {
            var posDiff = Mouse.GetState().Position.ToVector2() - Owner.GetPosition();
            posDiff.Normalize();
            return posDiff;
        }

        public KeyboardGameInput(ClientPlayerEntity owner) : base(owner)
        {
        }
    }
}