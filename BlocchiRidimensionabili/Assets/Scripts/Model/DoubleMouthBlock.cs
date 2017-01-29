using System.Collections;
using System;
namespace model
{
    public abstract class DoubleMouthBlock : Block
    {
        protected Block firstInnerNext, secondInnerNext;

        public void SetFirstInnerNext(Block block)
        {
            firstInnerNext = block;
        }
        public void SetSecondInnerNext(Block block)
        {
            secondInnerNext = block;
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