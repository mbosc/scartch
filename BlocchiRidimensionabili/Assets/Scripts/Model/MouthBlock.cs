using System.Collections;
using System;
namespace model
{
    public abstract class MouthBlock : Block
    {
        protected Block innerNext;

        public void SetInnerNext(Block block)
        {
            innerNext = block;
        }

        public void UnsetInnerNext()
        {
            innerNext = null;
        }
    }
}