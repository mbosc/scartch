using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{

	public class OptionWrapperButton : Selectable
	{
		public override void OnDeselection(){
			var wrap = transform.parent.GetComponent<OptionWrapper> ();
			if (wrap.showing)
				wrap.HideOptions ();
			else
				wrap.ShowOptions ();
		}
	}
}
