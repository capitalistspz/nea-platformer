using System.Collections.Generic;
using client2.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace client2.input
{
    public class KeyboardGameInput : GameInput<Keys>
    {
        public override bool IsPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public override Vector2 AimDirection
        {
            get
            {
                var ownerBounds = (RectangleF) Owner.Bounds;
                var posDiff = Mouse.GetState().Position - ownerBounds.Center;
                posDiff.Normalize();
                return posDiff;
            }
        }

        public KeyboardGameInput(ClientPlayerEntity owner) : base(owner)
        {
            AssignInput(DefaultMapping);
        }
        public static Dictionary<InputAction, Keys> DefaultMapping => new(8)
        {
            [InputAction.Jump] = Keys.Space,
            [InputAction.Attack1] = Keys.F,
            [InputAction.Attack2] = Keys.G,
            [InputAction.Block] = Keys.Z,
            [InputAction.Left] = Keys.A,
            [InputAction.Right] = Keys.D,
            [InputAction.Up] = Keys.W,
            [InputAction.Down] = Keys.S
                
        };
        public static Dictionary<InputAction, Keys> BlankMapping => new(8)
        {
            [InputAction.Jump] = Keys.None,
            [InputAction.Attack1] = Keys.None,
            [InputAction.Attack2] = Keys.None,
            [InputAction.Block] = Keys.None,
            [InputAction.Left] = Keys.None,
            [InputAction.Right] = Keys.None,
            [InputAction.Up] = Keys.None,
            [InputAction.Down] = Keys.None,
        };
    }
    
}