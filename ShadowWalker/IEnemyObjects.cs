using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShadowWalker
{
    abstract class IEnemyObjects
    {
        // This represents 3D models in memory.
        public int health { get; set; }
        public int armor { get; set; }


        public abstract void Update(PlayerObjects p, HeightMap hm);
        public abstract void Initialize();
        public abstract void Draw(Camera camera);
        public abstract Matrix GetWorld();

    }
}