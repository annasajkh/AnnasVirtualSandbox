using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using System;

namespace AnnasVirtualSandbox.Desktop.Scripts.Utils
{
    public class Sprite : GameObject
    {
        private Vector2 origin;

        public Texture2D texture;
        public Rectangle sourceRectangle;
        public Vector2 scale;
        public Color color;
        public float rotation;

        public Sprite(Texture2D texture, Rectangle sourceRectangle, Vector2 position, Vector2 scale, float rotation = 0) : base(position)
        {
            this.texture = texture;
            this.position = position;

            this.scale = scale;
            this.rotation = rotation;

            origin = new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f);
            this.sourceRectangle = sourceRectangle;
            color = Color.White;
        }

        public override void Update(float delta)
        {

        }

        public float GetWidth()
        {
            return sourceRectangle.Width * scale.X;
        }

        public float GetHeight()
        {
            return sourceRectangle.Height * scale.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(texture,
                             sourceRectangle: sourceRectangle,
                             position: position,
                             color: color,
                             rotation: rotation,
                             scale: scale,
                             origin: origin,
                             effects: spriteEffects,
                             layerDepth: 1);
        }
    }
}