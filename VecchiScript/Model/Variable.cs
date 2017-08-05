using System.Collections;
using System;
namespace model
{
    public abstract class Variable : Reference
    {
        private string name;
        public event EventHandler VariableChanged;
        public void OnVariableChanged()
        {
            if (VariableChanged != null)
                VariableChanged(this, EventArgs.Empty);
        }

        public string Name
        {
            get { return name; }
            set
            {
                // verificare che non sia in collisione con il namespace
                // per questo potrebbe convenire aggiungere un riferimento al dominio di definizione
                name = value;
            }
        }

        public abstract string EvaluateAsString();
    }
}