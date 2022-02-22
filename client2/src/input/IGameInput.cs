using client2.entities;
using Microsoft.Xna.Framework;

namespace client2.input
{
    // Interface made in order to treat all input types the same
    public interface IGameInput
    {
        public Vector2 AimDirection { get; }
        public ClientPlayerEntity Owner { get; set; }
        public void Update();
    }
}