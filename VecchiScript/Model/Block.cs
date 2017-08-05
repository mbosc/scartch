using System.Collections.Generic;
using System;
using System.Reflection;
namespace model
{
	public abstract class Block : ScriptingElement
    {
		
        protected Block next;
        

        public Block Next
        {
            get { return next; }
            set
            {
                next = value;
            }
        }

        public void UnsetNext()
        {
            next = null;
        }

        public abstract Block ExecuteAndGetNext();
    }
}