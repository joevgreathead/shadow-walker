using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Adding input and gamer services frameworks to use
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
    /// This class is to create an instace of a controller that uses a 
    /// PlayerIndex when Contructing in order to receve the proper player's
    /// information each update. This is because we have multiple players 
    /// sometimes, we need to have one controller per player.
    /// </summary>
    class Controller
    {
        #region classVariables
        public Vector4 DPAD;
        public Vector4 INPUTKEYS;

        private GamePadState padState { get; set; }
        private GamePadState prevPadState { get; set; }

        private KeyboardState keyState { get; set; }
        private KeyboardState prevKeyState { get; set; }

        private PlayerIndex pIndex { get; set; }
        #endregion
        public Controller(PlayerIndex playerIndex){
            DPAD = Vector4.Zero;
            INPUTKEYS = Vector4.Zero;
            this.pIndex = playerIndex;
            prevPadState = GamePad.GetState(this.pIndex);
            prevKeyState = Keyboard.GetState(this.pIndex);
        }
        public void Update(){
            //Reset control vectors to zero.
            DPAD = Vector4.Zero;
            INPUTKEYS = Vector4.Zero;
            //Pass along the old state and refresh the new state.
            prevPadState = padState;
            padState = GamePad.GetState(this.pIndex);
            prevKeyState = keyState;
            keyState = Keyboard.GetState(this.pIndex);
            //Check for action buttons
            if (pressY())
                INPUTKEYS.X = 1;
            if (pressA())
                INPUTKEYS.Y = 1;
            if (pressX())
                INPUTKEYS.Z = 1;
            if (pressB())
                INPUTKEYS.W = 1;
            //Checks and gets movement buttons.
            if (moveUp())
                DPAD.X = getUp();
            if (moveDown())
                DPAD.Y = getDown();
            if (moveLeft())
                DPAD.Z = getLeft();
            if (moveRight())
                DPAD.W = getRight();
        }
        /// <summary>
        /// Checks to see if button was just pressed this update.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns></returns>
        public bool wasButtonPressed(Buttons button){
            return (padState.IsButtonDown(button)
                && prevPadState.IsButtonUp(button));
        }
        /// <summary>
        /// Checks to see if button was released this update.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns></returns>
        public bool wasButtonreleased(Buttons button){
            return (padState.IsButtonUp(button)
                && prevPadState.IsButtonDown(button));
        }
        /// <summary>
        /// Checks to see if button is being held.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns></returns>
        public bool isHoldingButton(Buttons button){
            return (padState.IsButtonDown(button)
                && prevPadState.IsButtonDown(button));
        }
        /// <summary>
        /// Checks to see if key was just pressed this update.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns></returns>
        public bool wasKeyPressed(Keys key) {
            return (keyState.IsKeyDown(key)
                && prevKeyState.IsKeyUp(key));
        }
        /// <summary>
        /// Checks to see if key was released this update.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public bool wasKeyReleased(Keys key) { 
            return (keyState.IsKeyUp(key) &&
                prevKeyState.IsKeyDown(key));
        }
        /// <summary>
        /// Checks to see if key is being held.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns></returns>
        public bool isHoldingKey(Keys key) { 
            return (keyState.IsKeyDown(key) &&
                prevKeyState.IsKeyDown(key));
        }
        /// <summary>
        /// Checks for the thumbstick moving to the right 
        /// or the DPAD right being pressed.
        /// </summary>
        /// <returns></returns>
        public bool moveRight(){
            return (padState.ThumbSticks.Left.X > 0
                || padState.IsButtonDown(Buttons.DPadRight)
                || keyState.IsKeyDown(Keys.Right)
                || keyState.IsKeyDown(Keys.D));
        }
        /// <summary>
        /// Checks for the thumbstick moving to the left 
        /// or the DPAD left being pressed.
        /// </summary>
        /// <returns></returns>
        public bool moveLeft(){
            return (padState.ThumbSticks.Left.X < 0
                || padState.IsButtonDown(Buttons.DPadLeft)
                || keyState.IsKeyDown(Keys.Left)
                || keyState.IsKeyDown(Keys.A));
        }
        /// <summary>
        /// Checks for the thumbstick moving up 
        /// or the DPAD up being pressed.
        /// </summary>
        /// <returns></returns>
        public bool moveUp(){
            return (padState.ThumbSticks.Left.Y > 0
                || padState.IsButtonDown(Buttons.DPadUp)
                || keyState.IsKeyDown(Keys.Up)
                || keyState.IsKeyDown(Keys.W));
        }
        /// <summary>
        /// Checks for the thumbstick moving down
        /// or the DPAD down being pressed.
        /// </summary>
        /// <returns></returns>
        public bool moveDown(){
            return (padState.ThumbSticks.Left.Y < 0
                || padState.IsButtonDown(Buttons.DPadDown)
                || keyState.IsKeyDown(Keys.Down)
                || keyState.IsKeyDown(Keys.S));
        }
        /// <summary>
        /// Gets the value of the Left Thumbstick if moving right.
        /// Returns 1 if DPAD Right is being pressed.
        /// </summary>
        /// <returns></returns>
        public float getRight() {
            if (padState.IsButtonDown(Buttons.DPadRight))
                return 1.0f;
            else if (padState.ThumbSticks.Left.X > 0.0)
                return padState.ThumbSticks.Left.X;
            else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                return 1.0f;
            else return 0.0f;
        }
        /// <summary>
        /// Gets the value of the Left Thumbstick if moving Left.
        /// Returns 1 if DPAD Left is being pressed.
        /// </summary>
        /// <returns></returns>
        public float getLeft() {
            if (padState.IsButtonDown(Buttons.DPadLeft))
                return 1.0f;
            else if (padState.ThumbSticks.Left.X < 0.0)
                return padState.ThumbSticks.Left.X;
            else if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                return 1.0f;
            else return 0.0f;
        }
        /// <summary>
        /// Gets the value of the Left Thumbstick if moving up.
        /// Returns 1 if DPAD Up is being pressed.
        /// </summary>
        /// <returns></returns>
        public float getUp() {
            if (padState.IsButtonDown(Buttons.DPadUp))
                return 1.0f;
            else if (padState.ThumbSticks.Left.Y > 0.0)
                return padState.ThumbSticks.Left.Y;
            else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                return 1.0f;
            else return 0.0f;
        }
        /// <summary>
        /// Gets the value of the Left Thumbstick if moving down.
        /// Returns 1 if DPAD down is being pressed.
        /// </summary>
        /// <returns></returns>
        public float getDown() {
            if (padState.IsButtonDown(Buttons.DPadDown))
                return 1.0f;
            else if (padState.ThumbSticks.Left.Y < 0.0)
                return padState.ThumbSticks.Left.Y;
            else if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                return 1.0f;
            else return 0.0f;
        }
        /// <summary>
        /// Returns true if the Left Thumbstick is moving 
        /// in either the X or the Y direction.
        /// False if both directions equal 0.
        /// </summary>
        /// <returns></returns>
        public bool isMovingLeftStick() {
            return (padState.ThumbSticks.Left.X != 0 || padState.ThumbSticks.Left.Y != 0);
        }
        /// <summary>
        /// Returns true if the Right Thumbstick is moving 
        /// in either the X or the Y direction.
        /// False if both directions equal 0.
        /// </summary>
        /// <returns></returns>
        public bool isMovingRightStick() { 
            return (padState.ThumbSticks.Right.X != 0 || padState.ThumbSticks.Right.Y !=0);
        }
        /// <summary>
        /// Returns true if any button on the DPAD are being pressed.
        /// </summary>
        /// <returns></returns>
        public bool isPressingDPAD() {
            return (padState.IsButtonDown(Buttons.DPadUp) ||
                padState.IsButtonDown(Buttons.DPadDown) ||
                padState.IsButtonDown(Buttons.DPadLeft) ||
                padState.IsButtonDown(Buttons.DPadRight));
        }
        /// <summary>
        /// Checks to see if the length of the array of current 
        /// keys being pressed is greater than 0 signifying
        /// that a key is being pressed on the keyboard.
        /// </summary>
        /// <returns></returns>
        public bool isPressingKeys(){
            return (keyState.IsKeyDown(Keys.W)
                || keyState.IsKeyDown(Keys.S)
                || keyState.IsKeyDown(Keys.A)
                || keyState.IsKeyDown(Keys.D)
                || keyState.IsKeyDown(Keys.Up)
                || keyState.IsKeyDown(Keys.Down)
                || keyState.IsKeyDown(Keys.Left)
                || keyState.IsKeyDown(Keys.Right));
        }
        /// <summary>
        /// Returns true if any button on the input pad are being pressed.
        /// </summary>
        /// <returns></returns>
        public bool isPressingINPUT()
        {
            return (padState.IsButtonDown(Buttons.A) ||
                padState.IsButtonDown(Buttons.B) ||
                padState.IsButtonDown(Buttons.X) ||
                padState.IsButtonDown(Buttons.Y));
        }
        /// <summary>
        /// Checks for the A button being pressed.
        /// </summary>
        /// <returns></returns>
        public bool pressA() { 
            return (padState.IsButtonDown(Buttons.A) 
                || keyState.IsKeyDown(Keys.K));
        }
        /// <summary>
        /// Checks for the B button being pressed.
        /// </summary>
        /// <returns></returns>
        public bool pressB() {
            return (padState.IsButtonDown(Buttons.B)
                || keyState.IsKeyDown(Keys.L));
        }
        /// <summary>
        /// Checks for the X button being pressed.
        /// </summary>
        /// <returns></returns>
        public bool pressX() {
            return (padState.IsButtonDown(Buttons.X)
                || keyState.IsKeyDown(Keys.J));
        }
        /// <summary>
        /// Checks for the Y button being pressed.
        /// </summary>
        /// <returns></returns>
        public bool pressY() {
            return (padState.IsButtonDown(Buttons.Y)
                || keyState.IsKeyDown(Keys.I));
        }
        /// <summary>
        /// Checks to see if some form of Up was pressed just this update.
        /// </summary>
        /// <returns></returns>
        public bool wasUpPressed() {
            return (wasLeftStickToggledUp()
                || wasButtonPressed(Buttons.DPadUp)
                || wasKeyPressed(Keys.Up)
                || wasKeyPressed(Keys.W));
        }
        /// <summary>
        /// Checks to see if some form of Down was pressed just this update.
        /// </summary>
        /// <returns></returns>
        public bool wasDownPressed(){
            return (wasLeftStickToggledDown()
                || wasButtonPressed(Buttons.DPadDown)
                || wasKeyPressed(Keys.Down)
                || wasKeyPressed(Keys.S));
        }
        /// <summary>
        /// Checks to see if the left stick was pushed up just this update.
        /// </summary>
        /// <returns></returns>
        public bool wasLeftStickToggledUp(){
            return (prevPadState.ThumbSticks.Left.Y == 0
                && padState.ThumbSticks.Left.Y > 0);
        }
        /// <summary>
        /// Checks to see if the left stick was pushed down just this update.
        /// </summary>
        /// <returns></returns>
        public bool wasLeftStickToggledDown(){
            return (prevPadState.ThumbSticks.Left.Y == 0
                && padState.ThumbSticks.Left.Y < 0);
        }
    }
}
