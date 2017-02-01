using System;
using UnityEngine;
using model;
using System.Linq;
using System.Collections.Generic;

namespace view
{
	public class LookAtBlockWrapper : BlockWrapper
	{
		protected override void Start(){
			testo = "Rivolgiti verso {  }";
			base.Start ();
		}

		public override ActorWrapper Owner {
			get {
				return base.Owner;
			}
			set {
				base.Owner = value;
				block = new LookAtBlock (Owner.actor);
			}
		}

		private class LookAtBlock : Block
		{
            public Actor owner;
            public LookAtBlock(Actor owner)
            {
                this.owner = owner;
                Func<IList<object>> valuesCalculator = () =>
                {
                    List<object> res = new List<object>();
                    model.Environment.Instance.actors.ForEach(s => { if (s != owner) res.Add(s); });
                    model.Environment.Instance.controllers.ForEach(res.Add);
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

