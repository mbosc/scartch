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

        public void SetNext(Block block)
        {
            next = block;
        }

        public void UnsetNext()
        {
            next = null;
        }

        public virtual void Execute()
        {
            next.Execute();
        }
    }
}