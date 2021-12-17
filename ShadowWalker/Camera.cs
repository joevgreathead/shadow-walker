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
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }
        public Viewport viewport { get; set; }

        // Vectors for the view matrix
        Vector3 cameraPosition;
        Vector3 cameraDirection;
        Vector3 cameraUp;

        //Vector3 target;
        //Vector3 prevTarget;
        Vector3 difference;

        // Speed
        float speed = 3;
        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up, Viewport viewport)
            : base(game)
        {

            // Create view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            difference = pos - target;
            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 3000); // near plane, far plan of field of view.
            
            this.viewport = viewport;
        }

        

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void cameraUpdate(Vector3 target) {
            cameraPosition = target + difference;
            view = Matrix.CreateLookAt(cameraPosition,
                target, cameraUp);
        }
        public void MoveForwardBackward(bool forward)
        {
            // Move forward/backward
            if (forward)
                cameraPosition += cameraDirection * speed;
            else
                cameraPosition -= cameraDirection * speed;
        }
        public void MoveStrafeLeftRight(bool left)
        {
            //Strafe
            if (left)
            {
                cameraPosition +=
                    Vector3.Cross(cameraUp, cameraDirection) * speed;
            }
            else
            {
                cameraPosition -=
                    Vector3.Cross(cameraUp, cameraDirection) * speed;
            }

        }
        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition,
                cameraDirection, cameraUp);
        }

    }
}
