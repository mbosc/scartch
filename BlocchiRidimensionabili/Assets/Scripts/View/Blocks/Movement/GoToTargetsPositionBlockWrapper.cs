using System;
using UnityEngine;
using model;
using System.Collections.Generic;
using System.Linq;

namespace view
{
	public class GoToTargetsPositionBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			testo = "Vai alla posizione di {  }";
			block = new GoToTargetsPositionBlock (wrapper.actor);
			base.Init (wrapper, autoinit);
		}

		private class GoToTargetsPositionBlock : Block
		{
			
			public GoToTargetsPositionBlock(Actor owner){
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
                owner.Position = targetPosition;
				return Next;
			}
		}
	}
}

