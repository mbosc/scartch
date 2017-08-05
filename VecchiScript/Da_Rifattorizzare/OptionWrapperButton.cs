using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{

	public class OptionWrapperButton : LaserSelectable
	{
        public override void SelectA()
        {
            var wrap = transform.parent.GetComponent<OptionWrapper>();
            if (wrap.showing)
                wrap.HideOptions();
            else
                wrap.ShowOptions();
        }
    }
}
