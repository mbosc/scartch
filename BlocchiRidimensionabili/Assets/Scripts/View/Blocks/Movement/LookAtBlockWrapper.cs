using System;
using UnityEngine;
using model;
using System.Linq;
using System.Collections.Generic;

namespace view
{
	public class LookAtBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoMovimento;
			testo = "Rivolgiti verso {  }";
			block = new LookAtBlock (wrapper.actor);
			base.Init (wrapper, autoinit);
		}

		private class LookAtBlock : Block
		{
            
            public LookAtBlock(Actor owner)
            {
                this.owner = owner;
                Func<IList<object>> valuesCalculator = () =>
                {
                    List<object> res = new List<object>();
                    model.Environment.Instance.Actors.ForEach(s => { if (s != owner) res.Add(s); });
                    model.Environment.Instance.Controllers.ForEach(res.Add);
                    return res;
                };
                var opt = new Option(valuesCalculator);
                options.Add(0, opt);
            }
            public override Block ExecuteAndGetNext()
            {
                var targetPosition = (options[0].PossibleValues[options[0].chosenValue] as InteractionItem).Position;

                //TODO algoritmo qui.
                return Next;
            }
        }
	}
}

