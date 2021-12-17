using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace ShadowWalker
{
    public interface BasicAgentInterfaces
    {
        void Patrol();
        void Fight();
        void Flee();
        bool isSafe();
        bool runAway();
    }
}
