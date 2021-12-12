using System;
using System.Collections;
using common.events;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace common.entities
{
    public class PlayerEntity : BaseEntity
    {
        private bool _jump;
        private bool _onGround;
        public readonly string Name;
        public PlayerEntity(Vector2 position, string name) : 
            this(position, Guid.NewGuid()) 
            => (Name, MaxSpeed) = (name, new Size2(8, 8));

        public readonly Vector2 MaxSpeed;
        public PlayerEntity(Vector2 position, Guid guid) : base(position, guid)
        {
        }
        public void Update(GameTime gameTime)
        {
            var time = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 10);
            var oldPosition = GetPosition();
            var position = oldPosition + GetVelocity() * time; 
            SetVelocity(GetVelocity() * 0.9f);
            _onGround = false;
            _jump = false;
        }
        
        public void ApplyMovement(MovementEventArgs args)
        {
            SetPosition(args.Position);
            SetVelocity(args.Velocity);
        }

        public void ApplyInput(InputEventArgs args)
        {
            var actions = args.Actions;
            if (actions[0])
                Jump();
            SetVelocity(args.MovementDirection + GetVelocity());
        }

        public void Jump()
        {
            if (_onGround)
                AddVelocity(new Size2(0, 0.7f));
        }
        
    }
}