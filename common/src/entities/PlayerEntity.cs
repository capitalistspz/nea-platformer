using System;
using System.Collections;
using common.events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Serilog;

namespace common.entities
{
    public class PlayerEntity : BaseEntity
    {
        private bool _jump;
        private bool _onGround;
        private float _attackCooldown;
        private float _timeSinceLastAttack;
        public readonly string Name;
        public readonly Vector2 MaxSpeed;
        public static Texture2D PlayerTexture;
        public override IShapeF Bounds { get; }

        public PlayerEntity(Vector2 position, World world, string name) :
            this(position, world, Guid.NewGuid())
        {
            Name = name;
        }

        public PlayerEntity(Vector2 position, World world, Guid guid) : base(position, world, guid)
        {
            MaxSpeed = new Size2(8, 8);
            _jump = false;
            _attackCooldown = 100;
            Bounds = new RectangleF(position, new Size2(128, 128));
            SetVisibility(true);
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
            _timeSinceLastAttack += deltaTime;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, GetPosition(), Color.White);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is PlayerEntity)
            {
                Bounds.Position -= collisionInfo.PenetrationVector;
                collisionInfo.Other.Bounds.Position += collisionInfo.PenetrationVector;
                
                //var velocity = GetVelocity() - collisionInfo.PenetrationVector * 0.4f;
                //SetVelocity(velocity);
            }
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
            if (actions[2] && _timeSinceLastAttack > _attackCooldown)
            {
                _timeSinceLastAttack = 0;
                Shoot(args.AimDirection);
            }
                
            SetVelocity(args.MovementDirection + GetVelocity());
        }
        
        // Fires a projectile in the direction aimed at
        private void Shoot(Vector2 aimDirection)
        {
            var projectile = new SimpleProjectileEntity(((RectangleF) Bounds).Center, currentWorld)
            {
                DamageAmount = 1,
                OwnerId = Id
            };
            projectile.SetVelocity(aimDirection * 8);
            currentWorld.AddEntity(projectile);
        }

        public void Jump()
        {
            if (_onGround)
            {
                AddVelocity(new Vector2(0, 0.7f));
                _jump = true;
            }
                
        }

    }
}