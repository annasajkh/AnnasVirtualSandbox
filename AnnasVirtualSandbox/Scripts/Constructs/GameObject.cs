using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnnasVirtualSandbox.Desktop.Scripts.Constructs
{
    public abstract class GameObject
    {
        public Vector2 position;

        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public abstract void Update(float delta);

        public abstract void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects);
    }
}
