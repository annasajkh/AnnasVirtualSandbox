using Microsoft.Xna.Framework;
using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnnasVirtualSandbox.Desktop.Scripts.Utils
{
    public class RectangleCollider : GameObject
    {
        public Rectangle rectangle;
        Sprite sprite;

        public RectangleCollider(Vector2 position, int width, int height) : base(position)
        {
            rectangle = new Rectangle((int) (position.X - width * 0.5f), (int)(position.Y - width * 0.5f), width, height);
            sprite = new Sprite(texture: Game1.textureAtlas,
                                sourceRectangle: new Rectangle(Game1.textureSize * 2,
                                                               0,
                                                               Game1.textureSize,
                                                               Game1.textureSize),
                                position: new Vector2(position.X, position.Y), 
                                scale: new Vector2(width / Game1.textureSize, height / Game1.textureSize));
        }

        public override void Update(float delta)
        {

            rectangle.X = (int)(position.X - rectangle.Width * 0.5f);
            rectangle.Y = (int)(position.Y - rectangle.Height * 0.5f);

            sprite.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            sprite.Draw(spriteBatch, spriteEffects);
        }
    }
}
