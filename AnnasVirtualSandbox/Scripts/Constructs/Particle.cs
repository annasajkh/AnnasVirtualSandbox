using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AnnasVirtualSandbox.Desktop.Scripts.Utils;

namespace AnnasVirtualSandbox.Desktop.Scripts.Constructs
{
    public enum ParticleType
    {
        SAND,
        AIR,
        DIRT
    }

    public abstract class Particle : GameObject
    {
        public Sprite sprite;
        public ParticleType type;
        public RectangleCollider collider;

        public Particle(Vector2 position, ParticleType type, Sprite sprite) : base(position)
        {
            if(type != ParticleType.AIR)
            {
                collider = new RectangleCollider(new Vector2(position.X, position.Y),
                                                (int)(Game1.particleSize),
                                                (int)(Game1.particleSize));
            }
            this.sprite = sprite;
            this.type = type;
        }

        public override void Update(float delta)
        {
            if (type != ParticleType.AIR)
            {
                collider.position = position;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            //sprite.Draw(spriteBatch, spriteEffects);
            collider.Draw(spriteBatch, spriteEffects);
        }
    }
}
