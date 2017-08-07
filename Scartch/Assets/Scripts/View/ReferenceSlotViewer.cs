using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace View
{
    public class ReferenceSlotViewer : MonoBehaviour
    {
        private RefType type;

        public RefType Type
        {
            get { return type; }
            set { type = value; }
        }

        public event System.Action<int> SlotEmptied;
        public event System.Action<int, ReferenceViewer> SlotFilled;

        private int number;

        public int Number
        {
            set
            {
                number = value;
            }
        }

        //TODO
        // Methods to be implemented
    }
}
