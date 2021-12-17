using System;
using System.Collections.Generic;
using System.Linq;
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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Camera camera1 { get; protected set; }
        //public Camera camera2 { get; protected set; }
        //public Camera camera3 { get; protected set; }
        //public Camera camera4 { get; protected set; }
        public Camera currentDrawingCamera { get; protected set; }
        CharacterManager charManager;
        EnvironmentManager environManager;
        Menu menu;

        public GameTime gameTime;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferHeight = 900;
            //graphics.PreferredBackBufferWidth = 1440;
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create viewports
            Viewport vp1 = GraphicsDevice.Viewport;
            vp1.Height = (GraphicsDevice.Viewport.Height);
            vp1.Width = (GraphicsDevice.Viewport.Width);
            //Create Objects
            environManager = new EnvironmentManager(this);
            Components.Add(environManager);
            charManager = new CharacterManager(this);
            Components.Add(charManager);
            menu = new Menu(this);
            Components.Add(menu);
            
            // Add camera components
            camera1 = new Camera(this, new Vector3(300, 425, 300), 
                new Vector3(0,0,0) , 
                Vector3.Up, vp1);
            Components.Add(camera1);

            base.Initialize();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Load the heightmap the environmenManager loads.
            charManager.loadHeightMap(environManager.heightMap);
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (0 != menu.getState() || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //System.Console.WriteLine(environManager.heightMap.getHeight(charManager.players[0].position));
            

            //Updates the camera.
            camera1.cameraUpdate(charManager.players.First().translation.Translation);
            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Set current drawing camera for player 1
            // and set the viewport to player 1's viewport,
            // then clear and call base.Draw to invoke
            // the Draw method on the ModelManager component
            currentDrawingCamera = camera1;
            GraphicsDevice.Viewport = camera1.viewport;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
