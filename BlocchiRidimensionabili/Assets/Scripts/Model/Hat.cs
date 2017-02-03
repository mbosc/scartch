using System.Collections;
using System;
namespace model
{
	public class Hat : ScriptingElement
    {
		private Actor owner;
		public Actor Owner {
			get { return owner; }
		}
        private Block next;

		public Hat(Actor owner)
        {
			this.owner = owner;
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