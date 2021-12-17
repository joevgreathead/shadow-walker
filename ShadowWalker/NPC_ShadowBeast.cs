using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShadowWalker
{
    class NPC_ShadowBeast : IEnemyObjects 
    {
        // This represents 3D models in memory.
        public Model model { get; protected set; }

        // Get state list
        public enum beastBehavior {Idle, Passive, Alarm, Hostile, Fleeing}
        beastBehavior beastbehavior = beastBehavior.Idle;

        // Movement varibles
        private float speed = 0.01f;
        Vector3 position = Vector3.Zero;
        Matrix translation = Matrix.Identity;
        

        // This world for this particular model.
        protected Matrix world = Matrix.Identity;
        protected Matrix rotation = Matrix.Identity;
        protected Matrix MODELFIX = Matrix.CreateRotationX(0.5f * MathHelper.Pi);
        protected Quaternion modelRot = Quaternion.Identity;

        public NPC_ShadowBeast(Model m) // Constructor
        {
            model = m;
        }

        public override void Update(HeightMap hm) // Overridden by the children
        {
            //Code to adjust height of character to position on height map.
            //Checks to see if character is on height map first then
            //changes Y position by taking the height from getHeight() and adding
            //half the height of the model.
            if (hm.isOnHeightMap(this.position))
                this.position.Y = hm.getHeight(this.position); //add half the hight of the model here

            // rotation/translation updates
            rotation = Matrix.CreateFromQuaternion(modelRot);
            translation = Matrix.CreateTranslation(position);

            // Behavior switch
            getBehavior();
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
            return world * MODELFIX * rotation * translation; 
        }
        public void setBehavior(beastBehavior b)
        {
            switch (b)
            {
                case beastBehavior.Idle:
                    // have no state running in updater
                    runIdle();
                    //Console.Out.WriteLine("ShadowBeast is Idle");
                    break;
                case beastBehavior.Passive:
                    // have "wandering" state running in updater
                    runPassive();
                    Console.Out.WriteLine("ShadowBeast is Passive");
                    break;
                case beastBehavior.Alarm:
                    // have "alert others" state running in updater
                    runAlarm();
                    break;
                case beastBehavior.Hostile:
                    // have beast target random player and attack
                    runHostile();
                    break;
                case beastBehavior.Fleeing:
                    // have beast run back to spawn location when low on health
                    runFleeing();
                    break;

            }
        }
        public void getBehavior()
        {
            setBehavior(beastbehavior);
        }
        public void runIdle()
        {
            speed = 0;
        }
        public void runPassive()
        {
            speed = 1.5f; 
        }
        public void runAlarm()
        {
            speed = 0f;
        }
        public void runHostile()
        {
            speed = 1.5f;
        }
        public void runFleeing()
        {
            speed = 1.7f;
        }
    }
}
