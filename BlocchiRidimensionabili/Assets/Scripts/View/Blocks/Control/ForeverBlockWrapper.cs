using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
	public class ForeverBlockWrapper : MouthBlockWrapper
	{

		public override void Init(ActorWrapper wrapper, bool autoinit = true)
		{
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "Per sempre";
			block = new ForeverBlock();
            lastBlock = true;
			base.Init (wrapper, autoinit);
		}

		private class ForeverBlock : MouthBlock
		{


            private Block flux;
			public override Block ExecuteAndGetNext ()
			{
                if (flux == null)
                {
                    flux = innerNext.ExecuteAndGetNext();
                    return this;
                }
                else
                {
                    flux = flux.ExecuteAndGetNext();
                    return this;
                }
			}
		}
	}
}

