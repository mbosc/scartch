using System;
using UnityEngine;
using model;
using System.Collections.Generic;
using System.Linq;

namespace view
{
	public class GoToTargetsPositionBlockWrapper : BlockWrapper
	{
		protected override void Start(){
			testo = "Vai alla posizione di {  }";
			base.Start ();
		}

		public override ActorWrapper Owner {
			get {
				return base.Owner;
			}
			set {
				base.Owner = value;
				block = new GoToTargetsPositionBlock (Owner.actor);
			}
		}

		private class GoToTargetsPositionBlock : Block
		{
			public Actor owner;
			public GoToTargetsPositionBlock(Actor owner){
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
                owner.Position = targetPosition;
				return Next;
			}
		}
	}
}

