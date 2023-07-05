using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using AnnasVirtualSandbox.Desktop.Scripts.Utils;
using Microsoft.Xna.Framework;

namespace AnnasVirtualSandbox.Desktop.Scripts.Particles
{
    public class Sand : Particle
    {
        public Sand(Vector2 position) : 
        base(position, ParticleType.SAND, new Sprite(texture: Game1.textureAtlas,
                                                     sourceRectangle: new Rectangle(Game1.textureSize * 2,
                                                                                    0,
                                                                                    Game1.textureSize,
                                                                                    Game1.textureSize),
                                                     position: position,
                                                     scale: new Vector2(Game1.gameScale, Game1.gameScale)))
        {

        }

        public override void Update(float delta)
        {

            base.Update(delta);
        }
    }
}
