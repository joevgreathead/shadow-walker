using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ShadowWalker
{
    class BasicModel
    {
        // This represents 3D models in memory.
        public Model model { get; protected set; }

        
        // This world for this particular model.
        protected Matrix world = Matrix.Identity;


        public BasicModel(Model m) // Constructor
        {
            model = m;
        }

        public virtual void Update() // Overridden by the children
        {
        }

        public virtual void Initialize()
        {
        }

        public void Draw(Camera camera)
        {
            // Our spaceship model has only one mesh, but in the
            // future if you have multi-meshes, these two lines
            // makes sure that the meshes are drawn on the right place.
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
