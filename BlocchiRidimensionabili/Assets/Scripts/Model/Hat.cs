using System.Collections;
using System;
namespace model
{
    public class Hat
    {
        private Block next;

        public Hat()
        {
            next = null;
        }

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

        public virtual void Execute()
        {
			if (Next != null)
            	Environment.Instance.EvaluationEngine.AddFlux(Next);
        }
    }
}