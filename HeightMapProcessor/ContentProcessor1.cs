using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using TInput = Microsoft.Xna.Framework.Content.Pipeline.Graphics.Texture2DContent;
using TOutput = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelContent;

namespace HeightMapProcessor
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "HeightMap Processor")]
    public class ContentProcessor1 : ContentProcessor<TInput, TOutput>
    {
        #region Properties


        private float terrainScale = 30f;
        [DisplayName("Terrain Scale")]
        [DefaultValue(30f)]
        [Description("Scale of the the terrain geometry width and length.")]
        public float TerrainScale
        {
            get { return terrainScale; }
            set { terrainScale = value; }
        }

        private float terrainBumpiness = 640f;
        [DisplayName("Terrain Bumpiness")]
        [DefaultValue(640f)]
        [Description("Scale of the the terrain geometry height.")]
        public float TerrainBumpiness
        {
            get { return terrainBumpiness; }
            set { terrainBumpiness = value; }
        }

        private float texCoordScale = 0.1f;
        [DisplayName("Texture Coordinate Scale")]
        [DefaultValue(0.1f)]
        [Description("Terrain texture tiling density.")]
        public float TexCoordScale
        {
            get { return texCoordScale; }
            set { texCoordScale = value; }
        }

        private string terrainTextureFilename = "rocks.bmp";
        [DisplayName("Terrain Texture")]
        [DefaultValue("rocks.bmp")]
        [Description("The name of the terrain texture.")]
        public string TerrainTextureFilename
        {
            get { return terrainTextureFilename; }
            set { terrainTextureFilename = value; }
        }


        #endregion

        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            MeshBuilder builder = MeshBuilder.StartMesh("terrain");
            
            //Convert the input texture to float format, in order to process
            input.ConvertBitmapType(typeof(PixelBitmapContent<float>));

            PixelBitmapContent<float> heightMap;
            heightMap = (PixelBitmapContent<float>)input.Mipmaps[0];

            //Create the Terrain vertices
            for (int y = 0; y < heightMap.Height; y++){
                for (int x = 0; x < heightMap.Width; x++) {
                    Vector3 position;
                    //Position the vertices so that the heightfield is centered
                    // around x=0, z=0
                    position.X = terrainScale * (x - ((heightMap.Width - 1) / 2.0f));
                    position.Z = terrainScale * (y - ((heightMap.Height - 1) / 2.0f));

                    position.Y = (heightMap.GetPixel(x, y) - 1) * terrainBumpiness;
                    
                    builder.CreatePosition(position);
                }    
            }


            //Create a BasicMaterialContent and point it at our terrain texture.
            BasicMaterialContent material = new BasicMaterialContent();
            material.SpecularColor = new Vector3(.4f, .4f, .4f);

            string directory = Path.GetDirectoryName(input.Identity.SourceFilename);
            string texture = Path.Combine(directory, terrainTextureFilename);

            material.Texture = new ExternalReference<TextureContent>(texture);
            builder.SetMaterial(material);
            
            //Create a vertex channel for holding texture coordinates
            int texCoordId = builder.CreateVertexChannel<Vector2>(VertexChannelNames.TextureCoordinate(0));

            //Create the individual triangles that make up the terrain
            for (int y = 0; y < MathHelper.Min(heightMap.Height, 255) - 1; y++) {
                for (int x = 0; x < MathHelper.Min(heightMap.Width, 255) - 1; x++) {
                    AddVertex(builder, texCoordId, heightMap.Width, x, y);
                    AddVertex(builder, texCoordId, heightMap.Width, x + 1, y);
                    AddVertex(builder, texCoordId, heightMap.Width, x + 1, y + 1);

                    AddVertex(builder, texCoordId, heightMap.Width, x, y);
                    AddVertex(builder, texCoordId, heightMap.Width, x + 1, y + 1);
                    AddVertex(builder, texCoordId, heightMap.Width, x, y + 1);
                }
            }
            
            //Chain to the ModelProcessor so it can convert the mesh we gave it
            MeshContent terrainMesh = builder.FinishMesh();

            ModelContent model = context.Convert<MeshContent, ModelContent>(terrainMesh, "ModelProcessor");
            
            //Generate information about the heightmap and attach it
            //to the finished model's tag
            model.Tag = new HeightMapContent(heightMap, terrainScale, terrainBumpiness);

            return model;
        }
        void AddVertex(MeshBuilder builder, int texCoordId, int w, int x, int y)
        {
            builder.SetVertexChannelData(texCoordId, new Vector2(x, y) * texCoordScale);

            builder.AddTriangleVertex(x + y * w);
        }
    }
}