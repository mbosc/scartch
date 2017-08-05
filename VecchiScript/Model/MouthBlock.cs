using System.Collections;
using System;
namespace model
{
    public abstract class MouthBlock : Block
    {
        protected Block innerNext;
        private string valString;


        public Block InnerNext
        {
            get { return innerNext; }
            set { innerNext = value; }
        }

        public void UnsetInnerNext()
        {
            innerNext = null;
        }
    }
}