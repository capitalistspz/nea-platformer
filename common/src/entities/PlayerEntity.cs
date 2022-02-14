using System;
using System.Collections;
using common.events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            _jump = false;
        }
        public override void Update(GameTime gameTime)
        {
            
            var deltaTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 10);
            var oldPosition = GetPosition();
            var oldVelocity = GetVelocity();
            var position = oldPosition + oldVelocity * deltaTime; 
            
            SetPosition(position);
            
            var draggedVelocity = oldVelocity * 0.9f;
            SetVelocity(draggedVelocity.LengthSquared() < 0.009f ? Vector2.Zero : draggedVelocity);
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
            {
                AddVelocity(new Size2(0, 0.7f));
                _jump = true;
            }
                
        }
        
    }
}