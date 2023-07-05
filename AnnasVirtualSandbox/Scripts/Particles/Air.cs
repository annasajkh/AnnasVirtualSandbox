using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using Microsoft.Xna.Framework;

namespace AnnasVirtualSandbox.Desktop.Scripts.Particles
{
    public class Air : Particle
    {
        public Air(Vector2 position) :
        base(position, ParticleType.AIR, null)
        {
            
        }

        public override void Update(float delta)
        {

            base.Update(delta);
        }
    }
}
