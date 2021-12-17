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
    /// This class encapsulates the lists of players and enemies.
    /// This class is also responsible for drawing and updating all palyers and enemies.
    /// </summary>
    class CharacterManager : DrawableGameComponent
    {
        public List<PlayerObjects> players = new List<PlayerObjects>();
        public List<IEnemyObjects> npcs = new List<IEnemyObjects>();
        //The heightmap grabbed by loadHeightMap()
        //Use to adjust player and enemy heights to the terrain. 
        HeightMap heightMap;

        public CharacterManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
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
        protected override void LoadContent()
        {
            players.Add(new PlayerObjects(
                Game.Content.Load<Model>(@"models\test_puppet")));
            //npcs.Add(new NPC_ShadowHunter(
                //Game.Content.Load<Model>(@"models\shadowhunter"),(new Vector3(0,0,300))));
            npcs.Add(new NPC_ShadowGuardian(
              Game.Content.Load<Model>(@"models\shadowguardian"),(new Vector3(0,0,-300))));
            //npcs.Add(new NPC_ShadowLurker(
              //Game.Content.Load<Model>(@"models\shadowlurker"), (new Vector3(-300, 0, 0))));

            
            base.LoadContent();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Loop through all models and call Update()
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Update(heightMap);
            }
            for (int i = 0; i < npcs.Count; i++)
            {
                npcs[i].Update(players.First(), heightMap);
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            // Loop through all models and draw each model
            foreach (PlayerObjects bm in players)
            {
                bm.Draw(((Game1)Game).currentDrawingCamera);
            }
            foreach (IEnemyObjects bm in npcs)
            {
                bm.Draw(((Game1)Game).currentDrawingCamera);
            }

            base.Draw(gameTime);
        }
        /// <summary>
        /// Used to load a HeightMap object into CharacterManager one time
        /// to adjust the player and enemy heights.
        /// </summary>
        /// <param name="hm"></param>
        public void loadHeightMap(HeightMap hm) {
            this.heightMap = hm;
        }
    }
}
