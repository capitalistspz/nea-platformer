using System.Collections.Generic;
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

        public override Vector2 GetAimDirection
        {
            get
            {
                var posDiff = Mouse.GetState().Position.ToVector2() - Owner.GetPosition();
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
            [InputAction.Left] = Keys.Left,
            [InputAction.Right] = Keys.Right,
            [InputAction.Up] = Keys.Up,
            [InputAction.Down] = Keys.Down
                
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