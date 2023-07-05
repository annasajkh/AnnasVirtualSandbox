using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using Microsoft.Xna.Framework;
using System;
using AnnasVirtualSandbox.Desktop.Scripts.Particles;
using System.Collections.Generic;
using System.Linq;

namespace AnnasVirtualSandbox.Desktop.Scripts.Utils
{
    public static class WorldGeneration
    {
        private static Dictionary<Vector2, Chunk> ChunksTemp = new Dictionary<Vector2, Chunk>(Game1.renderDistance * 4);

        public static Particle[,] GenerateChunk(Vector2 position)
        {
            Particle[,] particles = new Particle[Game1.chunkSize, Game1.chunkSize];

            for (int i = 0; i < Game1.chunkSize; i++)
            {
                for (int j = 0; j < Game1.chunkSize; j++)
                {
                    float xOffset = position.X - Game1.particleSize;
                    float yOffset = position.Y - Game1.particleSize;

                    //multiply to add spacing 
                    xOffset += j * Game1.particleSize - Game1.chunkSize * Game1.particleSize * 0.5f;
                    yOffset += i * Game1.particleSize - Game1.chunkSize * Game1.particleSize * 0.5f;

                    float noise = OpenSimplexNoise2.Noise2_ImproveX(Game1.worldSeed, xOffset * 0.0005, yOffset * 0.0005);

                    particles[i, j] = noise > 0.0f ? (Particle) new Dirt(new Vector2(xOffset, yOffset)) : new Air(new Vector2(xOffset, yOffset));

                }
            }

            return particles;
        }

        public static int SnapToChunkPosition(float value)
        {
            return (int)(Math.Floor(value / (Game1.particleSize * Game1.chunkSize)) * Game1.particleSize * Game1.chunkSize);
        }

        private static Dictionary<Vector2, Chunk> Flood(Vector2 center)
        {
            Dictionary<Vector2, Chunk> brances = new Dictionary<Vector2, Chunk>(4);

            Vector2 position = new Vector2(center.X - (Game1.chunkSize * Game1.particleSize), center.Y);
            Chunk branch = new Chunk(position, null);
            brances.Add(position, branch);


            position = new Vector2(center.X + (Game1.chunkSize * Game1.particleSize), center.Y);
            branch = new Chunk(position, null);
            brances.Add(position, branch);


            position = new Vector2(center.X, center.Y + (Game1.chunkSize * Game1.particleSize));
            branch = new Chunk(position, null);
            brances.Add(position, branch);

            position = new Vector2(center.X, center.Y - (Game1.chunkSize * Game1.particleSize));
            branch = new Chunk(position, null);
            brances.Add(position, branch);

            return brances;
        }

        public static Dictionary<Vector2, Chunk> FloodFill(Vector2 position, bool fillstartPosition, int renderDistance)
        {
            Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

            if(fillstartPosition)
            {
                chunks.Add(position, new Chunk(position, null));
            }

            chunks = chunks.Concat(Flood(position)).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First().Value);


            for (ushort i = 0; i < renderDistance; i++)
            {
                foreach (var floodedChunk in chunks)
                {
                    ChunksTemp = ChunksTemp.Concat(Flood(floodedChunk.Value.position)).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First().Value);
                }

                chunks = chunks.Concat(ChunksTemp).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First().Value);

                ChunksTemp.Clear();
            }

            return chunks;
        }

        public static Dictionary<Vector2, Chunk> GenerateChunks(Vector2 position, bool fillstartPosition, ushort renderDistance)
        {
            position.X = SnapToChunkPosition(position.X);
            position.Y = SnapToChunkPosition(position.Y);

            Dictionary<Vector2, Chunk> chunks = FloodFill(position, fillstartPosition, renderDistance);

            foreach (var chunk in chunks)
            {
                int xSnap = SnapToChunkPosition(chunk.Value.position.X);
                int ySnap = SnapToChunkPosition(chunk.Value.position.Y);

                chunk.Value.particles = GenerateChunk(chunk.Value.position);
            }

            return chunks;
        }

        public static void RegenerateChunks(Vector2 position, Vector2 direction)
        {
            List<Vector2> edgeChunkKeys = new List<Vector2>();
            var keys = Game1.chunks.Keys;

            //find all the edge chunks
            foreach (var key in keys)
            {
                Vector2 right = new Vector2(key.X + (Game1.chunkSize * Game1.particleSize), key.Y);
                Vector2 left = new Vector2(key.X - (Game1.chunkSize * Game1.particleSize), key.Y);
                Vector2 up = new Vector2(key.X, key.Y - (Game1.chunkSize * Game1.particleSize));
                Vector2 down = new Vector2(key.X, key.Y + (Game1.chunkSize * Game1.particleSize));

                //Vector2 upRight = new Vector2(key.X + (Game1.chunkSize * Game1.particleSize), key.Y - (Game1.chunkSize * Game1.particleSize));
                //Vector2 upLeft = new Vector2(key.X - (Game1.chunkSize * Game1.particleSize), key.Y - (Game1.chunkSize * Game1.particleSize));
                //Vector2 downRight = new Vector2(key.X + (Game1.chunkSize * Game1.particleSize), key.Y + (Game1.chunkSize * Game1.particleSize));
                //Vector2 downLeft = new Vector2(key.X - (Game1.chunkSize * Game1.particleSize), key.Y + (Game1.chunkSize * Game1.particleSize));

                if (!(keys.Contains(right) && keys.Contains(left) && keys.Contains(up) && keys.Contains(down)))
                {
                    edgeChunkKeys.Add(key);
                }
            }

            //find equal chunk keys to the player direction
            List<Vector2> equalChunkKeys = new List<Vector2>();

            for (int i = 0; i < edgeChunkKeys.Count; i++)
            {
                float dotProduct = Vector2.Dot(Vector2.Normalize(edgeChunkKeys[i] - position), direction);

                if(dotProduct > 0.75)
                {
                    equalChunkKeys.Add(edgeChunkKeys[i]);
                }
            }

            try
            {
                Vector2 closestChunkKey = Util.FindClosesPosition(position, equalChunkKeys);

                Dictionary<Vector2, Chunk> chunks = GenerateChunks(Game1.chunks[closestChunkKey].position, false, Game1.renderDistance);

                Game1.chunks = Game1.chunks.Concat(chunks).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First().Value);
            }
            catch(Exception e)
            {
                var x = e;
            }
        }
    }
}