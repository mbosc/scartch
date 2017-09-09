using System.Collections;
using System.Collections.Generic;
using System;

namespace Scripting
{
    public class Flux
    {
        public static Flux current = null;

        public Flux(ScriptingElement initiator)
        {
            this.initiator = initiator;
        }

        public ScriptingElement initiator;

        private bool executing;

        public bool Executing
        {
            get { return executing; }
            set { executing = value; }
        }

        private Block currentBlock;

        public Block CurrentBlock
        {
            get { return currentBlock; }
            set { currentBlock = value; }
        }

        public event Action Callbacks;

        public void EndExecution()
        {
            Executing = false;
            if (Callbacks != null)
                Callbacks();
        }
    }
}
