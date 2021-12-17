using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace ShadowWalker
{
    /// <summary>
    /// This class contains and draws the terrain map for the current level loaded.
    /// </summary>
    class EnvironmentManager : DrawableGameComponent
    {
        Model model;
        public HeightMap heightMap;
        protected Matrix world = Matrix.Identity;

        public EnvironmentManager(Game game)
            : base(game)
        {
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //Load the terrain model using the HeightMapProcessor.
            model = Game.Content.Load<Model>("maps/terrain");
            //The model.tag contains a HeightMap object that we extrapolate.
            heightMap = model.Tag as HeightMap;

            base.LoadContent();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            drawModel(((Game1)Game).currentDrawingCamera);
            base.Draw(gameTime);
        }
        /// <summary>
        /// Function to help draw the terrain model.
        /// </summary>
        /// <param name="camera"></param>
        public void drawModel(Camera camera)
        {
            
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = camera.projection;
                    be.View = camera.view;
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
            
        }
        public virtual Matrix GetWorld()
        {
            return world;
        }

    }
}