using System.Collections;
using System;
namespace model
{
	public class Hat : ScriptingElement
    {


        public event Action PrepareForDetachment;

	
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

        protected void SubscribeToMessage(string message)
        {
            Environment.Instance.EvaluationEngine.AddSubscriber(message, this);
        }

        protected void UnsubscribeFromMessage(string message)
        {
            Environment.Instance.EvaluationEngine.RemoveSubscriber(message, this);
        }
    }
}