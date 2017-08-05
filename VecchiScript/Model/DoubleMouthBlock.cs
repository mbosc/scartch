using System.Collections;
using System;
namespace model
{
    public abstract class DoubleMouthBlock : Block
    {
        protected Block firstInnerNext, secondInnerNext;

        public Block FirstInnerNext
        {
            get { return firstInnerNext; }
            set { firstInnerNext = value; }
            
        }
        public Block SecondInnerNext
        {
            get { return secondInnerNext; }
            set { secondInnerNext = value; }
        }
        public void UnsetFirstInnerNext()
        {
            firstInnerNext = null;
        }
        public void UnsetSecondInnerNext()
        {
            secondInnerNext = null;
        }
    }
}