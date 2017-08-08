using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace Model
{
    public class Variable
    {
        public Variable()
        {
        }

        private int refCount;

        public int RefCount
        {
            get { return refCount; }
            set { refCount = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private RefType type;

        public RefType Type
        {
            get { return type; }
            set
            {
                if (RefCount > 0)
                    throw new VariableAlterationException("Variable " + Name + " has " + RefCount + " active references and its type cannot be changed.");
                type = value;
            }
        }

        public event System.Action<string> NameChanged, ValueChanged;
        public event System.Action<RefType> TypeChanged;
        public event System.Action Destroyed;

        public void Destroy()
        {
            if (RefCount > 0)
                throw new VariableAlterationException("Variable " + Name + " has " + RefCount + " active references and cannot be destroyed.");
            if (Destroyed != null)
                Destroyed();
        }
    }

    [Serializable]
    internal class VariableAlterationException : Exception
    {
        public VariableAlterationException()
        {
        }

        public VariableAlterationException(string message) : base(message)
        {
        }

        public VariableAlterationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VariableAlterationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}