using System.Collections;
using System.Collections.Generic;
using System;

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
            set { type = value; }
        }

        public event System.Action<string> NameChanged, ValueChanged;
        public event System.Action<RefType> TypeChanged;
        public event System.Action Destroyed;

        public void Destroy()
        {
            if (Destroyed != null)
                Destroyed();
        }
    }
}