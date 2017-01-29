using System.Collections.Generic;
using System;
namespace model
{
    public abstract class Expression
    {
        private Dictionary<int, Reference> references = new Dictionary<int, Reference>();

        public void AddReference(int n, Reference reference)
        {
            references.Add(n, reference);
        }
        public void RemoveReference(int n)
        {
            references.Remove(n);
        }
    }
}