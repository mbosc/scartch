using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view {
    public class OnPlayModeHatWrapper : HatWrapper {

        // Use this for initialization
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			hat = new Hat(wrapper.actor);
            Environment.Instance.PlayModeStarted += hat.Execute;
            testo = "Al play mode";
			base.Init (wrapper, autoinit);
        }


        
    }
}