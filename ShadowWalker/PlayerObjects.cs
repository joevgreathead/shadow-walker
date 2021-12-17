using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ShadowWalker
{
    class PlayerObjects
    {
        #region PlayerObjects Variables
        enum PlayerState
        {
            attackMelee,
            attackRange,
            defenseBlock,
            actionWalk,
            actionAction
        }

        // This represents 3D models in memory.
        public Model model { get; protected set; }
        
        PlayerIndex pIndex; //The player number.
        Controller gamePad; //The gamepad for that player.
        private float playerSpeed = 10.0f; //Player's speed for running/walking.

        // The world for this particular model.
        protected Matrix world = Matrix.Identity;
        protected Matrix rotation = Matrix.Identity;
        protected Matrix scale = Matrix.CreateScale(2.0f);
        protected Matrix MODELFIX = Matrix.CreateRotationX(0.5f * MathHelper.Pi);
        protected Quaternion modelRot = Quaternion.Identity;
        
        public Vector3 position = Vector3.Zero;
        public Matrix translation = Matrix.Identity;

        //Shader to change color via state.
        Vector3 ambientColor = Vector3.Zero;
        PlayerState pState = PlayerState.actionWalk;

        BoundingSphere bBox;

        WeaponBehaviorManager weaponBehaviorManager;

        #endregion PlayerObjects Variables
        public PlayerObjects(Model m) // Constructor
        {
            model = m;
            this.pIndex = PlayerIndex.One;
            gamePad = new Controller(this.pIndex);

            bBox.Center = this.position;
            bBox.Radius = 10.0f;

            weaponBehaviorManager = new WP_M4();
        }
        public void Initialize()
        {
            
        }
        /// <summary>
        /// Allows the player to update itself.
        /// </summary>
        /// <param name="hm">A HeightMap object used to adjust the height of the player to the terrain.</param>
        public void Update(HeightMap hm)
        {
            //Update the controller's state to grab new input.
            gamePad.Update();
            checkState();
            
            //Code to adjust height of character to position on height map.
            //Checks to see if character is on height map first then
            //changes Y position by taking the height from getHeight() and adding
            //half the height of the model.
            if (hm.isOnHeightMap(this.position))
                this.position.Y = hm.getHeight(this.position) + 5.0f; //add half the hight of the model here
            
            if (gamePad.isMovingLeftStick() || gamePad.isPressingDPAD() || gamePad.isPressingKeys()){
                Quaternion newRot = Quaternion.CreateFromAxisAngle(Vector3.Up, findDirection());
                modelRot = newRot;
                if(boundaryCheck(hm))// && moveOverArea(hm))
                    this.position += moveForward();
            }

            
            bBox.Center = this.position;
            rotation = Matrix.CreateFromQuaternion(modelRot);
            translation = Matrix.CreateTranslation(position);
        }
        /// <summary>
        /// Draws the player model.
        /// </summary>
        /// <param name="camera"></param>
        public void Draw(Camera camera)
        {
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
        /// <summary>
        /// Returns the world rotated and translated for this model.
        /// </summary>
        /// <returns></returns>
        public virtual Matrix GetWorld()
        {
            return world * scale * MODELFIX * rotation * translation;
        }
        /// <summary>
        /// Finds the direction as a float by either finding the ARC Tangent
        /// or using the findDirection_DPAD function if the DPAD is being pressed.
        /// </summary>
        /// <returns></returns>
        private float findDirection() {
            if (gamePad.isPressingDPAD() || gamePad.isPressingKeys())
                return findDirection_DPAD();
            else
                return (((float)Math.Atan2(gamePad.DPAD.X + gamePad.DPAD.Y, gamePad.DPAD.Z + gamePad.DPAD.W)) + (0.75f * MathHelper.Pi));
        }
        /// <summary>
        /// Finds the angle of direction as a float
        /// if the DPAD is being used to move.
        /// </summary>
        /// <returns></returns>
        private float findDirection_DPAD() {
            float x = 0.0f;
            float y = 0.0f;

            if (gamePad.DPAD.X > 0)
                x = 1.0f;
            else if (gamePad.DPAD.Y > 0)
                x = -1.0f;

            if (gamePad.DPAD.Z > 0)
                y = -1.0f;
            else if (gamePad.DPAD.W > 0)
                y = 1.0f;

            return (((float)Math.Atan2(x, y)) + (0.75f * MathHelper.Pi));
        }
        /// <summary>
        /// Moves the model forward in the direction it is facing.
        /// </summary>
        private Vector3 moveForward() {
            Vector3 forwardVec = Vector3.Transform(new Vector3(0, 0, 1), this.modelRot);
            return (forwardVec * playerSpeed);
        }
        /// <summary>
        /// Checks to see if position the player is moving to is on the 
        /// height map and returns true or false based on that.
        /// </summary>
        /// <param name="hm">Height map to check position on.</param>
        /// <returns></returns>
        private bool boundaryCheck(HeightMap hm) {
            Vector3 newPosition = this.position + moveForward();
            return (hm.isOnHeightMap(newPosition));
        }
        /*
        private bool moveOverArea(HeightMap hm){
            Vector3 newPosition = this.position + moveForward();
            float currentHeight = hm.getHeight(this.position);
            float newHeight = hm.getHeight(newPosition);
            if (currentHeight < newHeight)
                return (currentHeight + 250 > newHeight);
            else if (currentHeight > newHeight)
                return (currentHeight - 250 < newHeight);
            else return false ;
        }
        */
        /// <summary>
        /// Updates the player's state depending on what INPUT 
        /// keys are received from the controller.
        /// </summary>
        private void checkState() {
            //if (gamePad.isPressingKeys() || gamePad.isPressingINPUT())
                if (gamePad.INPUTKEYS.X > 0)
                    pState = PlayerState.actionAction;
                else if (gamePad.INPUTKEYS.Y > 0)
                    pState = PlayerState.attackMelee;
                else if (gamePad.INPUTKEYS.Z > 0)
                    pState = PlayerState.attackRange;
                else if (gamePad.INPUTKEYS.W > 0)
                    pState = PlayerState.defenseBlock;
                else pState = PlayerState.actionWalk;
            //Update the ambientColor
            updateState();
        }
        /// <summary>
        /// Updates ambientColor to reflect state that the player is in.
        /// </summary>
        private void updateState() {
            switch (pState) { 
                case PlayerState.actionAction:
                    ambientColor = Color.Yellow.ToVector3();
                    break;
                case PlayerState.attackMelee:
                    ambientColor = Color.Green.ToVector3();
                    break;
                case PlayerState.attackRange:
                    ambientColor = Color.Blue.ToVector3();
                    this.attack();
                    break;
                case PlayerState.defenseBlock:
                    ambientColor = Color.Red.ToVector3();
                    break;
                default:
                    ambientColor = Vector3.Zero;
                    break;
            }
        }
        /// <summary>
        /// Invookes the useWeapon method of the current weapon
        /// behavior manager. I.E. It attacks with the currently 
        /// equipped weapon.
        /// </summary>
        public void attack() { 
            weaponBehaviorManager.useWeapon(); 
        }

    }
}