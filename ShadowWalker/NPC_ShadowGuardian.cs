using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShadowWalker
{
    class NPC_ShadowGuardian : IEnemyObjects, BasicAgentInterfaces
    {
        // This represents 3D models in memory.
        public Model model { get; protected set; }
        public BoundingSphere bBox; 

        // Get state list
        public enum hunterBehavior { Idle, Passive, Alarm, Hostile, Fleeing }
        // This world for this particular model.

        protected Matrix world = Matrix.Identity;

        #region General Model Stuff
        public Vector3 playerPosition;
        public Vector3 myPosition;
        public Quaternion myRotation;
        public Vector3 myScale;
        public Matrix translation = Matrix.Identity;
        public Matrix scale = Matrix.CreateScale(.25f);
        protected Matrix MODELFIX = Matrix.CreateRotationX(0.5f * MathHelper.Pi);

        float movAngle = 0.0f;
        Vector3 ambientColor = Vector3.Zero;

        #endregion

        #region Some Simple Physics
        public Vector3 velocity = Vector3.Zero;
        public float speed = .01f;
        #endregion

        #region AI Related Stuff
        public float HP = 1000;

        private BasicModelAgent agent;
        public string State
        {
            get { return agent.State().Substring(23); }
        }
        #endregion


        public NPC_ShadowGuardian(Model m, Vector3 pos) // Constructor
        {
            model = m;

            myPosition = pos;
            myRotation = new Quaternion(0, 0, 0, 1);
            myScale = new Vector3(1, 1, 1);

            bBox.Radius = 30.0f;

            // Set the AI agent up with an initial state.
            agent = new BasicModelAgent(this, new State_Patrol());
        }

        public override void Update(PlayerObjects p, HeightMap hm)
        {
            // This is basicly the healing rate of out NPC. Each update it gets .125 hits back.
            if (HP < 100)
                HP += .125f;

            //Code to adjust height of character to position on height map.
            //Checks to see if character is on height map first then
            //changes Y position by taking the height from getHeight() and adding
            //half the height of the model.
            if (hm.isOnHeightMap(this.myPosition))
                this.myPosition.Y = hm.getHeight(this.myPosition) + 5.0f; //add half the hight of the model here

            playerPosition = p.position;
            translation = Matrix.CreateTranslation(myPosition);
            // This is the AI bit, Execute the agents current state.
            agent.ExecuteState();

            bBox.Center = this.myPosition;

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
                    be.AmbientLightColor = ambientColor;
                    be.Projection = camera.projection;
                    be.View = camera.view;
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        public override Matrix GetWorld()
        {
            return world * scale * Matrix.CreateFromQuaternion(myRotation) * translation;
        }
        
        #region General model behavior stuff
        public virtual void Translate(Vector3 distance)
        {
            myPosition += Vector3.Transform(distance, Matrix.CreateFromQuaternion(new Quaternion(0, 0, 0, 1)));
        }
        public virtual void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, Matrix.CreateFromQuaternion(myRotation));
            myRotation = Quaternion.Normalize(Quaternion.CreateFromAxisAngle(axis, angle) * myRotation);
        }
        public void LookAt(Vector3 target, float speed)
        {
            Vector3 tminusp = target - myPosition;
            Vector3 ominusp = Vector3.Backward;

            tminusp.Normalize();

            float theta = (float)System.Math.Acos(Vector3.Dot(tminusp, ominusp));
            Vector3 cross = Vector3.Cross(ominusp, tminusp);

            cross.Normalize();

            Quaternion targetQ = Quaternion.CreateFromAxisAngle(cross, theta);
            myRotation = Quaternion.Slerp(myRotation, targetQ, speed);
        }

        private void Move()
        {
            // Calulate the distance I need to travel.
            Vector3 distance = velocity * speed;

            // Find out my direction of facing as I move.
            Vector3 target = Vector3.Transform(distance, Matrix.CreateFromQuaternion(new Quaternion(0, 0, 0, 1)));
            target += myPosition;
            LookAt(target, .1f);

            // Move.
            Translate(distance);

        }
        #endregion

        #region IPatrolObject Methods
        /// <summary>
        /// Method called to patrol
        /// </summary>
        public void Patrol()
        {
            Console.Out.WriteLine("I am patroling");
            // Set the model to a passive color
            ambientColor = Color.Cyan.ToVector3();

            // adjust the angle to move to.
            movAngle += 0.01f;
            // Make sure I am at a slow speed.
            speed = .05f;

            // Adjust my velocity, you could have some really funky path finding in here.
            // Maybe something for another tutorial eh..?
            velocity.X = 20 * (float)Math.Cos(movAngle);
            velocity.Z = 20 * (float)Math.Sin(movAngle);

            Move();
            velocity.Normalize();
        }
        /// <summary>
        /// Yes, you guessed it my fight method.
        /// </summary>
        public void Fight()
        {
            // This is where you would put all your lovely shoot the Player and dive for cover stuff
            // and maybe some more path finding to get a better shot on your target.
            Console.Out.WriteLine("I am fighting");
            // Simulate that I am taking damage. 
            HP -= .5f;
            
            // Set to a threatenign color.
            ambientColor = Color.Red.ToVector3();
            
            
            float distanceToPlayer = Vector3.Distance(myPosition, playerPosition);

            // Stop and fight if player is within distance.
            if (distanceToPlayer < 70.0f)
            {
                LookAt(playerPosition, .1f);
                speed = 0.0f;
            }
            // Otherwise chase after player.
            else
            {
                // Look at the Player with an angry face.... or in this case prop.
                LookAt(playerPosition, .1f);
                speed = 0.1f;
                velocity.Z *= speed;
                Move();
            }
            //Move();
            //Engage(playerPosition);

        }
        /// <summary>
        /// Guess what this method is for??
        /// </summary>
        public void Flee()
        {
            Console.Out.WriteLine("I am fleeing");
            // Run really fast!
            speed = .02f;

            // Yellow belly!
            ambientColor = Color.Yellow.ToVector3();

            // Probably be better to again pathfind your way out, but this is a simple tut, so just run!
            velocity.Z -= 1.0f;

            Move();
        }

        /// <summary>
        /// Simply, am I in any danger, or have I come into contact with the player in this case.
        /// </summary>
        /// <returns>true if safe else false.</returns>
        public bool isSafe()
        {
            bool retVal = true;

            // Am I in range of the player or are my hits too low?
            if (Vector3.Distance(myPosition, playerPosition) <= 150 || HP < 70)
                retVal = false;

            return retVal;
        }

        /// <summary>
        /// Do I think I should do a runner??
        /// </summary>
        /// <returns>true, "yes I should leave.". false "naa I am OK"</returns>
        public bool runAway()
        {
            // If I have lost half or more of my hits, I want to leave...
            if (HP <= 50)
                return true;
            else
                return false;
        }
        #endregion

        #region AddedFunctions 5/4/10
        private void Engage(Vector3 enemy)
        {
            Console.Out.WriteLine("I am engaging");
            LookAt(enemy, .1f);
            float distanceFromPlayer = Vector3.Distance(myPosition, enemy);
            float engageSpeed = .05f;
            Vector3 distance;
            // Move.
            if (distanceFromPlayer > 80)
            {
                // Calulate the distance I need to travel.
                distance = velocity * engageSpeed;
                // Find out my direction of facing as I move.
                LookAt(enemy, .1f);
            }
            else
            {
                // Stop movement when in attack range.
                distance = Vector3.Zero;
                LookAt(enemy, .1f);

            }
            Translate(distance);

        }
        #endregion
    }
}
