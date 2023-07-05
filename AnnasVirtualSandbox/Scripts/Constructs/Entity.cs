using AnnasVirtualSandbox.Desktop.Scripts.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnnasVirtualSandbox.Desktop.Scripts.Constructs
{

    public class Entity : GameObject
    {
        public Vector2 velocity;
        public Vector2 movementDirection;
        protected Sprite sprite;
        public RectangleCollider collider;

        public float speed;
        public Vector2 maxVelocity = new Vector2(1000, 1000);

        Chunk chunkRightUp;
        Chunk chunkRightDown;
        Chunk chunkLeftUp;
        Chunk chunkLeftDown;

        Chunk[] collidingChunks;

        public Entity(Vector2 position, float speed, Sprite sprite = null, RectangleCollider collider = null) : base(position)
        {
            velocity = new Vector2();
            movementDirection = new Vector2();

            this.speed = speed;
            this.sprite = sprite;
            this.collider = collider;

            if(this.sprite == null)
            {
                this.sprite = new Sprite(texture: Game1.textureAtlas,
                                         sourceRectangle: new Rectangle(Game1.textureSize * 3,
                                                               0,
                                                               Game1.textureSize,
                                                               Game1.textureSize),
                                          position: new Vector2(position.X, position.Y),
                                          scale: new Vector2(Game1.particleSize / Game1.textureSize, Game1.particleSize / Game1.textureSize));
            }

            collidingChunks = new Chunk[4];
        }

        public void Resolve(RectangleCollider other)
        {

            if (collider.rectangle.Intersects(other.rectangle))
            {
                Rectangle collision = Rectangle.Intersect(collider.rectangle, other.rectangle);

                sprite.color = Color.Red;

                if(collision.Width != 0 && collision.Height != 0)
                {

                    if (collision.Width < collision.Height && position.X > other.position.X)
                    {
                        position.X += collision.Width;
                        velocity.X = 0;
                    }
                    else if (collision.Width > collision.Height && position.Y > other.position.Y)
                    {
                        position.Y += collision.Height;
                        velocity.Y = 0;
                    }

                    if (collision.Width < collision.Height && position.X < other.position.X)
                    {
                        position.X -= collision.Width;
                        velocity.X = 0;

                    }
                    else if (collision.Width > collision.Height && position.Y < other.position.Y)
                    {
                        position.Y -= collision.Height;
                        velocity.Y = 0;
                    }
                }
            }
        }

        public void ResolveCollision()
        {
            GetCollidingChunks();

            if (!collidingChunks.Contains(chunkRightUp))
            {
                Util.ChangeChunkColor(collidingChunks[0], Color.White);
                collidingChunks[0] = chunkRightUp;
            }

            if (!collidingChunks.Contains(chunkLeftUp))
            {
                Util.ChangeChunkColor(collidingChunks[1], Color.White);
                collidingChunks[1] = chunkLeftUp;
            }

            if (!collidingChunks.Contains(chunkRightDown))
            {
                Util.ChangeChunkColor(collidingChunks[2], Color.White);
                collidingChunks[2] = chunkRightDown;
            }

            if (!collidingChunks.Contains(chunkLeftDown))
            {
                Util.ChangeChunkColor(collidingChunks[3], Color.White);
                collidingChunks[3] = chunkLeftDown;
            }

            foreach (var chunk in collidingChunks)
            {
                if (chunk != null)
                {
                    foreach (var particle in chunk.particles)
                    {

                        if (particle.type != ParticleType.AIR)
                        {
                            particle.sprite.color = Color.Red;
                            Resolve(particle.collider);
                        }
                    }
                }
            }
        }

        public void GetCollidingChunks()
        {
            HashSet<Chunk> collidingCHunks = new HashSet<Chunk>();

            float halfWidth = sprite.GetWidth() * (Game1.particleSize * 0.05f);
            float halfHeight = sprite.GetHeight() * (Game1.particleSize * 0.05f);


            float xChunkRightUp = WorldGeneration.SnapToChunkPosition(position.X + halfWidth + Game1.chunkSize * Game1.particleSize * 0.5f);
            float yChunkRightUp = WorldGeneration.SnapToChunkPosition(position.Y - halfHeight + Game1.chunkSize * Game1.particleSize * 0.5f);


            float xChunkLeftUp = WorldGeneration.SnapToChunkPosition(position.X - halfWidth + Game1.chunkSize * Game1.particleSize * 0.5f);
            float yChunkLeftUp = WorldGeneration.SnapToChunkPosition(position.Y - halfHeight + Game1.chunkSize * Game1.particleSize * 0.5f);


            float xChunkRightDown = WorldGeneration.SnapToChunkPosition(position.X + halfWidth + Game1.chunkSize * Game1.particleSize * 0.5f);
            float yChunkRightDown = WorldGeneration.SnapToChunkPosition(position.Y + halfHeight + Game1.chunkSize * Game1.particleSize * 0.5f);


            float xChunkLeftDown = WorldGeneration.SnapToChunkPosition(position.X - halfWidth + Game1.chunkSize * Game1.particleSize * 0.5f);
            float yChunkLeftDown = WorldGeneration.SnapToChunkPosition(position.Y + halfHeight + Game1.chunkSize * Game1.particleSize * 0.5f);

            try
            {
                chunkRightUp = Game1.chunks[new Vector2(xChunkRightUp, yChunkRightUp)];
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                chunkRightDown = Game1.chunks[new Vector2(xChunkLeftUp, yChunkLeftUp)];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                chunkLeftUp = Game1.chunks[new Vector2(xChunkRightDown, yChunkRightDown)];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                chunkLeftDown = Game1.chunks[new Vector2(xChunkLeftDown, yChunkLeftDown)];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void Update(float delta)
        {

            if(movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }


            velocity += movementDirection * speed;
            velocity.Y += Game1.gravity;

            velocity = Vector2.Clamp(velocity, -maxVelocity, maxVelocity);

            velocity *= Game1.friction;

            position += velocity * delta;

            collider.position = position;
            collider.Update(0);

        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {

            ResolveCollision();

            sprite.position = position;

            sprite.Draw(spriteBatch, spriteEffects);
            //collider.Draw(spriteBatch, spriteEffects);
        }
    }
}
