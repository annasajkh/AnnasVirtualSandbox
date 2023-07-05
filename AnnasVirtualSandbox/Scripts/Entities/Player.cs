using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using AnnasVirtualSandbox.Desktop.Scripts.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AnnasVirtualSandbox.Desktop.Scripts.Entities
{
    public class Player : Entity
    {

        public Camera2D camera;
        public bool regenerateChunks;

        private float mouseScrollTemp;

        Vector2 regenerateChunksTrigger;


        public Player(Vector2 position, Camera2D camera, float speed) 
            : base(position, speed)
        {
            this.camera = camera;
            sprite = new Sprite(texture: Game1.textureAtlas,
                                sourceRectangle: new Rectangle(0,
                                                               Game1.textureSize * 6,
                                                               Game1.textureSize,
                                                               Game1.textureSize * 2),
                                position: position, 
                                scale: new Vector2(Game1.gameScale, Game1.gameScale));

            collider = new RectangleCollider(new Vector2(position.X, position.Y),
                                            (int)(Game1.particleSize),
                                            (int)(Game1.particleSize * 2));

            regenerateChunksTrigger = position;
        }

        public void GetInput(KeyboardState keyboardState, MouseState mouseState)
        {
            //--------------Zooming---------------
            if(Math.Abs(mouseState.ScrollWheelValue - mouseScrollTemp) > 0.0001f)
            {

                if (mouseState.ScrollWheelValue - mouseScrollTemp < 0)
                {
                    camera.zoom -= Game1.zoomSpeed * Game1.player.camera.zoom * Game1.zoomFactor;
                }
                else
                {
                    camera.zoom += Game1.zoomSpeed * Game1.player.camera.zoom * Game1.zoomFactor;
                }

                mouseScrollTemp = mouseState.ScrollWheelValue;
            }

            mouseScrollTemp = mouseState.ScrollWheelValue;
            //------------------------------------

            if (keyboardState.IsKeyDown(Keys.A))
            {
                movementDirection.X = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                movementDirection.X = 1;
            }
            else
            {
                movementDirection.X = 0;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                movementDirection.Y = -1;
            }
            else
            {
                movementDirection.Y = 0;
            }
        }

        public override void Update(float delta)
        {
            sprite.color = Color.White;

            base.Update(delta);
            //camera.position = position;

            //---------Smooth Camera Movement------------
            float dirX = position.X - camera.position.X;
            float dirY = position.Y - camera.position.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            if (!(length <= 0.0001f))
            {
                if(Math.Abs(length) > 0.0001f)
                {
                    dirX /= length;
                    dirY /= length;
                }
                else
                {
                    dirX = 0;
                    dirY = 0;
                }
                camera.position.X += dirX * length * Game1.cameraSmoothFactor * delta;
                camera.position.Y += dirY * length * Game1.cameraSmoothFactor * delta;
            }
            //--------------------------------------

            /*Trigger regenerating chunks when the distance between regenerateChunksTrigger 
             * and this player position is greater than some constant
            */
            float regenerateChunksBetween = (regenerateChunksTrigger - position).LengthSquared();

            if(regenerateChunksBetween > Game1.regenerateChunksRadius * Game1.regenerateChunksRadius)
            {
                regenerateChunksTrigger = position;
                regenerateChunks = true;
            }
        }

    }
}
