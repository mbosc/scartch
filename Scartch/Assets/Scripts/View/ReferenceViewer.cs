using System.Collections;
using System.Collections.Generic;
using Scripting;
using UnityEngine;

namespace View
{
    public class ReferenceViewer
    {
        public Reference Reference { get; set; }
        private Model.RefType type;

        public Model.RefType Type
        {
            get { return type; }
            set { type = value; //TODO gestire cambio di aspetto}
        }

    }
}
