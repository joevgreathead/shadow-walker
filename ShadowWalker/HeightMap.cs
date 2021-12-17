using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ShadowWalker
{
    public class HeightMap
    {
        private float scale;
        public float[,] heights;
        private Vector3 heightMapPosition;
        private float mapWidth;
        private float mapHeight;

        public HeightMap(float[,] heightInfo, float terrainScale ) {
            this.heights = heightInfo;
            this.scale = terrainScale;

            mapWidth = (heightInfo.GetLength(0) - 1) * terrainScale;
            mapHeight = (heightInfo.GetLength(1) - 1) * terrainScale;

            heightMapPosition.X = -(heightInfo.GetLength(0) - 1) / 2 * terrainScale;
            heightMapPosition.Z = -(heightInfo.GetLength(1) - 1) / 2 * terrainScale;
        }
        /// <summary>
        /// Returns true or false if a given position is on the heightmap.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool isOnHeightMap(Vector3 position) {
            Vector3 positionOnMap = position - heightMapPosition;

            return(positionOnMap.X > 0 
                && positionOnMap.X < mapWidth
                && positionOnMap.Z > 0
                && positionOnMap.Z < mapHeight);
        }
        /// <summary>
        /// Returns the height on the heightMap at a given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public float getHeight(Vector3 position) {
            Vector3 positionOnMap = position - heightMapPosition;

            int left;
            int top;
            left = (int)positionOnMap.X / (int)scale;
            top = (int)positionOnMap.Z / (int)scale;

            float xNormal = (positionOnMap.X % scale) / scale;
            float zNormal = (positionOnMap.Z % scale) / scale;

            float topHeight = MathHelper.Lerp(
                                            heights[left, top],
                                            heights[left + 1, top],
                                            xNormal);
            float botHeight = MathHelper.Lerp(
                                            heights[left, top + 1],
                                            heights[left + 1, top + 1],
                                            xNormal);
            return (MathHelper.Lerp(topHeight, botHeight, zNormal));
        }

    }

    /// <summary>
    /// This class will load the HeightMapInfo when the game starts. This class needs 
    /// to match the HeightMapInfoWriter.
    /// </summary>
    public class HeightMapInfoReader : ContentTypeReader<HeightMap>
    {
        protected override HeightMap Read(ContentReader input,
            HeightMap existingInstance)
        {
            float terrainScale = input.ReadSingle();
            int width = input.ReadInt32();
            int height = input.ReadInt32();
            float[,] heights = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    heights[x, z] = input.ReadSingle();
                }
            }
            return new HeightMap(heights, terrainScale);
        }
    }

}
