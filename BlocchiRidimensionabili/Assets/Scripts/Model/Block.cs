using System.Collections.Generic;
using System;
namespace model
{
    public abstract class Block
    {
        protected Block next;
        protected Dictionary<int, Reference> references = new Dictionary<int, Reference>();

        public void AddReference(int n, Reference reference)
        {
            references.Add(n, reference);
        }
        public void RemoveReference(int n)
        {
            references.Remove(n);
        }

        public void SetNext(Block block)
        {
            next = block;
        }

        public void UnsetNext()
        {
            next = null;
        }

        public abstract void Execute();
    }
}