using AnnasVirtualSandbox.Desktop.Scripts.Entities;
using AnnasVirtualSandbox.Desktop.Scripts.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnnasVirtualSandbox.Desktop
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static Texture2D textureAtlas;

        public static Player player;

        public static float cameraSmoothFactor = 5f;
        public static float zoomFactor = 0.1f;
        public static float zoomSpeed = 1f;
        public static int gameScale = 3;
        public static float gravity = 100;
        public static float friction = 0.85f;

        public static int textureSize = 16;
        public static int particleSize = textureSize * gameScale;

        public static ushort chunkSize = 16;
        public static ushort renderDistance = 6;
        public static ushort regenerateChunksRadius = (ushort)(renderDistance * chunkSize * particleSize  * 0.5f);
        public static long worldSeed;
        public static Random random;


        public static Dictionary<Vector2, Chunk> chunks;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            Window.Title = "Annas Virtual Sandbox";

            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;

            base.Initialize();
            random = new Random();
            worldSeed = random.Next();

            player = new Player(position: new Vector2(0, 0),
                                camera: new Camera2D(viewport: GraphicsDevice.Viewport,
                                                     position: new Vector2(0, 0),
                                                     rotation: 0,
                                                     zoom: 1),
                                speed: 300);

            chunks = WorldGeneration.GenerateChunks(player.position, true, renderDistance);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureAtlas = Content.Load<Texture2D>("texture_atlas");
        }

        protected override void UnloadContent()
        {
            textureAtlas.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //foreach (var key in chunks.Keys)
            //{
            //    chunks[key].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            //}

            player.GetInput(Keyboard.GetState(), Mouse.GetState());
            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if(player.regenerateChunks)
            {
                WorldGeneration.RegenerateChunks(player.position, player.movementDirection);
                player.regenerateChunks = false;
            }


            var keys = chunks.Keys.ToArray();

            for (int i = chunks.Count - 1; i >= 0; i--)
            {
                float lengthSquared = (player.position - keys[i]).LengthSquared();

                if(lengthSquared > (regenerateChunksRadius * 2.5) * (regenerateChunksRadius * 2.5))
                {
                    chunks.Remove(keys[i]);
                }
            }

            if (chunks.Count == 0)
            {
                chunks = WorldGeneration.GenerateChunks(player.position, true, renderDistance);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                              samplerState: SamplerState.PointClamp,
                              transformMatrix: player.camera.GetViewMatrix());


            foreach(var key in chunks.Keys)
            {
                chunks[key].Draw(spriteBatch, SpriteEffects.None);
            }

            player.Draw(spriteBatch, SpriteEffects.None);

            //player.regenerateTrigger.Draw(spriteBatch, SpriteEffects.None);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
