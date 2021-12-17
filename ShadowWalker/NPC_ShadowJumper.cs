using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShadowWalker
{
    class NPC_ShadowJumper : IEnemyObjects 
    {
        // This represents 3D models in memory.
        public Model model { get; protected set; }

        // Get state list
        public enum jumperBehavior { Idle, Passive, Alarm, Hostile, Fleeing }

        // This world for this particular model.
        protected Matrix world = Matrix.Identity;


        public NPC_ShadowJumper(Model m) // Constructor
        {
            model = m;
        }

        public override void Update(HeightMap hm) // Overridden by the children
        {
        }

        public override void Initialize()
        {
        }

        public override void Draw(Camera camera)
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
        public override Matrix GetWorld()
        {
            return world;
        }
        public void setBehavior(jumperBehavior b)
        {
            switch (b)
            {
                case jumperBehavior.Idle:
                    // have no state running in updater
                    break;
                case jumperBehavior.Passive:
                    // have "wandering" state running in updater
                    break;
                case jumperBehavior.Alarm:
                    // have "alert others" state running in updater
                    break;
                case jumperBehavior.Hostile:
                    // have beast target random player and attack
                    break;
                case jumperBehavior.Fleeing:
                    // have beast run back to spawn location when low on health
                    break;

            }
        }
    }
}
