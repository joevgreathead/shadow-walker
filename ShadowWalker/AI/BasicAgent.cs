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
    public class BasicAgent
    {
        int id; // Unique id for this agent.
        static int nextValidID;

        // State the current state.
        protected BasicState currentState;

        // Associated object.
        protected Object worldObject;

        // Get ID.
        public int ID
        {
            get { return id; }
        }

        // Constructor: Associated Object, and Current State
        public BasicAgent(Object worldObject, BasicState startState)
        {
            this.worldObject = worldObject;
            currentState = startState;
        }

        // Setting the ID.

        private void SetID()
        {
            id = nextValidID++;
        }

        // Method to change states
        public virtual void ChangeState(BasicState state)
        {
            // Exit this state.
            currentState.Exit(this);
            // Set new state.
            currentState = state;
            // Call the new state
            currentState.Enter(this);
        }

        // Basic check, "Am I safe?" : True = safe
        public virtual bool isSafe()
        {
            return true;
        }

        // Basic check, "Do i need to flee?" : True = yes
        public virtual bool runAway()
        {
            return false;
        }

        // Run the state we are in.
        public void ExecuteState()
        {
            currentState.Execute(this);
        }

        // Run away
        public virtual void Fleeing()
        { }

        // Attack
        public virtual void Fight()
        { }

        // Patrol

        public virtual void Patrol()
        { }

    }
}
