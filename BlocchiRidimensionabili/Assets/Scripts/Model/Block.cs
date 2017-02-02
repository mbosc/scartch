using System.Collections.Generic;
using System;
using System.Reflection;
namespace model
{
    public abstract class Block
    {
        protected Block next;
        protected Dictionary<int, Reference> references = new Dictionary<int, Reference>();
        protected Dictionary<int, Option> options = new Dictionary<int, Option>();

        public void AddReference(int n, Reference reference)
        {
            references.Add(n, reference);
        }

        public void RemoveReference(int n)
        {
            references.Remove(n);
        }

        public T GetReferenceAs<T>(int n)
        {
            try
            {
                return (T)references[0].GetType().GetMethod("Evaluate").Invoke(references[0], null);
            } catch (KeyNotFoundException) {
                return default(T);
            }
        }

        private T defaultGeneric<T>()
        {
            return default(T);
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

        public abstract Block ExecuteAndGetNext();
    }
}