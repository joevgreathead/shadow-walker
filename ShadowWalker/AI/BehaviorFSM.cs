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
    public class State_Flee : BasicState
    {
        public override void Execute(BasicAgent agent)
        {
            if (agent.isSafe())
                agent.ChangeState(new State_Patrol());
            else
                agent.Fleeing();

            base.Execute(agent);
        }
    }

    public class State_Fight : BasicState
    {
        public override void Execute(BasicAgent agent)
        {
            if (agent.isSafe())
                agent.ChangeState(new State_Patrol());
            else
            {
                if (agent.runAway())
                    agent.ChangeState(new State_Flee());
                else
                    agent.Fight();
            }

            base.Execute(agent);
        }
    }
    public class State_Patrol : BasicState
    {
        public override void Execute(BasicAgent agent)
        {
            if (!agent.isSafe())
                agent.ChangeState(new State_Fight());
            else
                agent.Patrol();

            base.Execute(agent);
        }
    }
}
