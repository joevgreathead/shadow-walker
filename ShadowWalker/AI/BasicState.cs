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
    // This is the base state class, when creating any state this class will be inherited
    public class BasicState
    {
        public BasicState()
        { }

        // Called when the state is entered.
        public virtual void Enter(BasicAgent agent)
        { }

        // Called when the stae is executed. Main function for AI logic to be called from.
        public virtual void Execute(BasicAgent agent)
        { }

        // Called when the state is exited.
        public virtual void Exit(BasicAgent agent)
        { }
    }
}
