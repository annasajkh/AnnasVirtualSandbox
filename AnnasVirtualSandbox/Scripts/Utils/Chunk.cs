using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AnnasVirtualSandbox.Desktop.Scripts.Utils
{
    public class Chunk : GameObject
    {
        public Particle[,] particles;
        //public bool debug = false;

        public Chunk(Vector2 position, Particle[,] particles) : base(position)
        {
            this.particles = particles;
        }

        public override void Update(float delta)
        {
            foreach (var particle in particles)
            {
                particle.Update(delta);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            //if(debug)
            //{
            //    foreach(var particle in particles)
            //    {
            //        if (particle.type != ParticleType.AIR)
            //        {
            //            particle.sprite.color = Color.Red;
            //        }
            //    }
            //}


            foreach (var particle in particles)
            {
                if(particle.type != ParticleType.AIR)
                {
                    particle.Draw(spriteBatch, spriteEffects);
                }
            }
        }
    }
}
