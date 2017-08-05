using System.Collections.Generic;
using System;
namespace model
{
    public abstract class Expression
    {
        private Dictionary<int, Reference> references = new Dictionary<int, Reference>();
        protected Dictionary<int, Option> options = new Dictionary<int, Option>();

        public void AddReference(int n, Reference reference)
        {
            references.Add(n, reference);
        }
        public void RemoveReference(int n)
        {
            references.Remove(n);
        }
        public Option GetOption(int i)
        {
            Option ex = options[i];
            return ex;
        }

        public int ReferencesCount
        {
            get { return references.Keys.Count; }
        }

        public T GetReferenceAs<T>(int n)
        {
            try
            {
                if (typeof(T) == typeof(string))
                    return (T)references[n].GetType().GetMethod("EvaluateAsString").Invoke(references[n], null);
                else
                    return (T)references[n].GetType().GetMethod("Evaluate").Invoke(references[n], null);
            }
            catch (KeyNotFoundException)
            {
                return default(T);
            }
        }
    }
}