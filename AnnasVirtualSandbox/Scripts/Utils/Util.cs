using System.Collections.Generic;
using AnnasVirtualSandbox.Desktop.Scripts.Constructs;
using Microsoft.Xna.Framework;

namespace AnnasVirtualSandbox.Desktop.Scripts.Utils
{
    public class Util
    {

        public static Vector2 FindClosesPosition(Vector2 target, List<Vector2> positions)
        {
            float smallest = float.MaxValue;
            Vector2 closest = positions[0];

            for (int i = 0; i < positions.Count; i++)
            {
                float lengthSquared = (positions[i] - target).LengthSquared();

                if (lengthSquared < smallest)
                {
                    smallest = lengthSquared;
                    closest = positions[i];
                }
            }

            return closest;
        }

        public static void ChangeChunkColor(Chunk chunk, Color color)
        {
            if (chunk != null)
            {
                foreach (var particle in chunk.particles)
                {

                    if (particle.type != ParticleType.AIR)
                    {
                        particle.sprite.color = color;
                    }
                }
            }
        }
    }
}
