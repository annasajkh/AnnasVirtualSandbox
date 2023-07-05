using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AnnasVirtualSandbox.Desktop.Scripts.Constructs;


namespace AnnasVirtualSandbox.Desktop.Scripts.Utils
{
    public class Camera2D : GameObject
    {
        private readonly Viewport viewport;
        public Vector2 origin;

        public float rotation;
        public float zoom;

        public Camera2D(Viewport viewport, Vector2 position, float rotation, float zoom) : base(position)
        {
            this.viewport = viewport;
            this.position = position;
            this.rotation = rotation;
            this.zoom = zoom;

            origin = new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-(position - origin), 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                   Matrix.CreateRotationZ(rotation) *
                   Matrix.CreateScale(zoom, zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(origin, 0.0f));
        }

        public override void Update(float delta)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new System.NotImplementedException();
        }
    }
}
