using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowWalker
{
    // Agent for models to impliment AI
    class BasicModelAgent : BasicAgent
    {
        public BasicModelAgent(Object worldObject, BasicState startState)
            : base(worldObject, startState)
        {
        }

        // Method to get the current state.
        public string State()
        {
            return this.currentState.ToString();
        }

        // AI dependent on it's safety and needing to run or not.
        public override bool isSafe()
        {
            return ((BasicAgentInterfaces)worldObject).isSafe();
        }
        public override bool runAway()
        {
            return ((BasicAgentInterfaces)worldObject).runAway();
        }
        public override void Patrol()
        {
            ((BasicAgentInterfaces)worldObject).Patrol();
        }
        public override void Fight()
        {
            ((BasicAgentInterfaces)worldObject).Fight();
        }
        public override void Fleeing()
        {
            ((BasicAgentInterfaces)worldObject).Flee();
        }
    }
}
