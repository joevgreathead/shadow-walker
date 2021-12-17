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
    public class Menu : DrawableGameComponent
    {
        #region Enum & Struct
        private enum Screen
        {
            Credits = 0,
            GameOver,
            Hud,
            Inventory,
            Main,
            Pause,
            Purchase,
            Score,
            Title,
            Exit
        }
        struct DisplayMarker
        {
            public int menuOpt;
            public int x;
            public int y;
        }
        #endregion Enum
        #region Class Variables
        Screen currentScreen = Screen.Title;
        Controller gamePad { get; set; }

        SpriteBatch spriteBatch;
        Texture2D displayCredits;
        Texture2D displayGameOver;
        Texture2D displayHUD;
        Texture2D displayInventory;
        Texture2D displayMain;
        Texture2D displayPause;
        Texture2D displayPurchase;
        Texture2D displayScore;
        Texture2D displayTitle;

        Texture2D displayMarker;
        DisplayMarker menuMarker;
        int creditY;
        

        #endregion Class Variables

        public Menu(Game game)
            : base(game)
        {
            
        }
        public override void Initialize()
        {
            menuMarker.menuOpt = 0;
            menuMarker.x = -500;
            menuMarker.y = -500;

            creditY = 0;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            displayCredits = Game.Content.Load<Texture2D>("menus/displayCredits");
            displayHUD = Game.Content.Load<Texture2D>("menus/displayhud");
            displayInventory = Game.Content.Load<Texture2D>("menus/displayinventory");
            displayMain = Game.Content.Load<Texture2D>("menus/displayMain");
            displayPause = Game.Content.Load<Texture2D>("menus/displaypause");
            displayTitle = Game.Content.Load<Texture2D>("menus/displayTitle");

            displayMarker = Game.Content.Load<Texture2D>("menus/displaymarker");
            
            gamePad = new Controller(PlayerIndex.One);
            
            base.LoadContent();
        }
        protected override void UnloadContent()
        {
            
        }
        public override void Update(GameTime gameTime){
            gamePad.Update();
            switch (currentScreen)
            {
                case Screen.Credits:    updateCredits();
                    break;
                case Screen.GameOver:   updateGameOver();
                    break;
                case Screen.Hud:        updateHUD();             
                    break;
                case Screen.Inventory:  updateInventory();            
                    break;
                case Screen.Main:       updateMain();    
                    break;
                case Screen.Pause:      updatePause();        
                    break;
                case Screen.Purchase:   updatePurchase();
                    break;
                case Screen.Score:      updateScore();
                    break;
                case Screen.Title:      updateTitle();
                    break;
                default:
                    break;
            }
            if (currentScreen != Screen.Main && currentScreen != Screen.Pause)
                setMarkerCoord(0);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //spriteBatch.GraphicsDevice.Clear(Color.White);
            
            switch (currentScreen)
            {
                case Screen.Credits:    spriteBatch.Draw(displayCredits, new Rectangle(0, creditY, displayCredits.Width, displayCredits.Height), Color.White);
                    break;
                case Screen.GameOver:
                    break;
                case Screen.Hud:        spriteBatch.Draw(displayHUD, new Rectangle(0, 0, displayHUD.Width, displayHUD.Height), Color.White);
                    break;
                case Screen.Inventory:  spriteBatch.Draw(displayInventory, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;
                case Screen.Main:       spriteBatch.Draw(displayMain, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                                        spriteBatch.Draw(displayMarker, new Rectangle(menuMarker.x, menuMarker.y, displayMarker.Width, displayMarker.Height), Color.White);
                    break;
                case Screen.Pause:      spriteBatch.Draw(displayPause, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                                        spriteBatch.Draw(displayMarker, new Rectangle(menuMarker.x, menuMarker.y, displayMarker.Width, displayMarker.Height), Color.White);
                    break;
                case Screen.Purchase:
                    break;
                case Screen.Score:
                    break;
                case Screen.Title:      spriteBatch.Draw(displayTitle, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;
                default:
                    break;
            }

            spriteBatch.End();
            
            GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RenderState.DepthBufferEnable = false;

            GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;

            GraphicsDevice.RenderState.AlphaTestEnable = true;
            GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
            GraphicsDevice.RenderState.ReferenceAlpha = 0;

            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Clamp;

            GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Linear;
            GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Linear;
            GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Linear;

            GraphicsDevice.SamplerStates[0].MipMapLevelOfDetailBias = 0.0f;
            GraphicsDevice.SamplerStates[0].MaxMipLevel = 0;

            GraphicsDevice.RenderState.DepthBufferEnable = true;
            GraphicsDevice.RenderState.AlphaBlendEnable = false;
            GraphicsDevice.RenderState.AlphaTestEnable = false;

            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            
            base.Draw(gameTime);
        }
        /// <summary>
        /// Changes the state to whatever state we want.
        /// 0 - Credits
        /// 1 - Game Over Screen
        /// 2 - HUD (gameplay running)
        /// 3 - Inventory Screen
        /// 4 - Main Menu
        /// 5 - Pause Screen
        /// 6 - Purchase Screen
        /// 7 - Score & Stats
        /// 8 - Title
        /// </summary>
        /// <param name="menuNumber">Number for which state to change to.</param>
        public void changeState(int menuNumber){
            switch (menuNumber) {
                case 0: currentScreen = Screen.Credits;
                    break;
                case 1: currentScreen = Screen.GameOver;
                    break;
                case 2: currentScreen = Screen.Hud;
                    break;
                case 3: currentScreen = Screen.Inventory;
                    break;
                case 4: currentScreen = Screen.Main;
                    break;
                case 5: currentScreen = Screen.Pause;
                    break;
                case 6: currentScreen = Screen.Purchase;
                    break;
                case 7: currentScreen = Screen.Score;
                    break;
                case 8: currentScreen = Screen.Title;
                    break;
            }
        }
        /// <summary>
        /// Update function for Credits screen. 
        /// Checks to see if "B" button was pressed to send it back 
        /// to the Main Menu.
        /// </summary>
        private void updateCredits() {
            if (gamePad.wasButtonPressed(Buttons.B) || gamePad.wasKeyPressed(Keys.B))
                currentScreen = Screen.Main;
            if(creditY > -1200)
                creditY--;
        }
        private void updateGameOver() { }
        /// <summary>
        /// Update function for the HUD.
        /// Checks for "Back" & "Start" being presed to switch to
        /// Inventory and Pause screen respectively.
        /// </summary>
        private void updateHUD() {
            if (gamePad.wasButtonPressed(Buttons.Start) || gamePad.wasKeyPressed(Keys.E) ){
                currentScreen = Screen.Pause;
            } else if (gamePad.wasButtonPressed(Buttons.Back) || gamePad.wasKeyPressed(Keys.Q) ){
                    currentScreen = Screen.Inventory;
            }
        }
        /// <summary>
        /// Update function for the inventory screen.
        /// Checks to see if "Back" button is pressed to return to HUD.
        /// </summary>
        private void updateInventory() {
            if (gamePad.wasButtonPressed(Buttons.Back) || gamePad.wasKeyPressed(Keys.Q))
                currentScreen = Screen.Hud;
        }
        /// <summary>
        /// Update function for the Main Menu.
        /// Changes the selection on screen, and checks to see if a selection
        /// is made by pressing A.
        /// </summary>
        private void updateMain(){
            setMarkerCoord(menuMarker.menuOpt + 1);
            if (gamePad.wasDownPressed()){
                menuMarker.menuOpt++;
                checkMenuOpt();
                System.Console.WriteLine(menuMarker.menuOpt);
            }else if (gamePad.wasUpPressed()){
                menuMarker.menuOpt--;
                checkMenuOpt();
                System.Console.WriteLine(menuMarker.menuOpt);
            }
            if (gamePad.wasButtonPressed(Buttons.A) || gamePad.wasKeyPressed(Keys.A)){
                if (menuMarker.menuOpt == 0){
                    currentScreen = Screen.Hud;
                    menuMarker.menuOpt = 0;
                }else if (menuMarker.menuOpt == 1){
                    currentScreen = Screen.Hud;
                    menuMarker.menuOpt = 0;
                }else if (menuMarker.menuOpt == 2){
                    currentScreen = Screen.Credits;
                    menuMarker.menuOpt = 0;
                }
            }

        }
        /// <summary>
        /// Update function for the Pause screen.
        /// Checks to see if "Start" button is pressed to return to HUD.
        /// Changes the selection on screen, and checks to see if a selection
        /// is made by pressing A.
        /// </summary>
        private void updatePause(){
            setMarkerCoord(menuMarker.menuOpt + 1);
            if (gamePad.wasButtonPressed(Buttons.Start) || gamePad.wasKeyPressed(Keys.E))
                currentScreen = Screen.Hud;
            if (gamePad.wasDownPressed()){
                menuMarker.menuOpt++;
                checkMenuOpt();
                System.Console.WriteLine("Pause" + menuMarker.menuOpt);
            }else if (gamePad.wasUpPressed()){
                menuMarker.menuOpt--;
                checkMenuOpt();
                System.Console.WriteLine("Pause" + menuMarker.menuOpt);
            }
            if (gamePad.wasButtonPressed(Buttons.A) || gamePad.wasKeyPressed(Keys.A)){
                if (menuMarker.menuOpt == 0){
                    //Place Load Game HERE!!
                    currentScreen = Screen.Hud;
                    menuMarker.menuOpt = 0;
                }else if (menuMarker.menuOpt == 1){
                    //Place Save Game HERE!!
                    currentScreen = Screen.Hud;
                    menuMarker.menuOpt = 0;
                }else if (menuMarker.menuOpt == 2){
                    currentScreen = Screen.Exit;
                    menuMarker.menuOpt = 0;
                }
            }
        }
        private void updatePurchase() { }//PURCHASE STUFF GOES HERE
        private void updateScore() { }//SCORE STUFF GOES HERE
        /// <summary>
        /// Update function for the Title Screen.
        /// Checks to see if "A" was pressed to advance to the next screen.
        /// </summary>
        private void updateTitle(){
            if (gamePad.wasButtonPressed(Buttons.A) || gamePad.wasKeyPressed(Keys.A)){
                this.currentScreen = Screen.Main;
            }
        }
        /// <summary>
        /// Returns a 1 or 0 to show if the exit selection
        /// has been made in the pause menu.
        /// 0 - Game still running.
        /// 1 - Exit selected.
        /// </summary>
        /// <returns></returns>
        public int getState() {
            int rtn;
            if (Screen.Exit != currentScreen)
                rtn = 0;
            else rtn = 1;
            return rtn;
        }
        /// <summary>
        /// Checks the MenuOpt variable and keeps it within it's range.
        /// </summary>
        private void checkMenuOpt(){
            if (menuMarker.menuOpt < 0)
                menuMarker.menuOpt = 0;
            else if (menuMarker.menuOpt > 2)
                menuMarker.menuOpt = 2;
        }
        /// <summary>
        /// Sets the x and y coordinates for drawing the marker
        /// based on a selection.
        /// 0 - Off Screen
        /// 1 - First Selection
        /// 2 - Second Selection
        /// 3 - Third Selection
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        private void setMarkerCoord(int sel) {
            if (sel == 0) {
                menuMarker.x = -500;
                menuMarker.y = -500;
            }else if (sel == 1) {
                menuMarker.x = 225;
                menuMarker.y = 250;
            }else if (sel == 2) {
                menuMarker.x = 225;
                menuMarker.y = 300;
            }else if (sel == 3) {
                menuMarker.x = 225;
                menuMarker.y = 350;
            }

        }
        
    }
}