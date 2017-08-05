using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view {
    public class OnPlayModeHatWrapper : HatWrapper {

        // Use this for initialization
        public override void Init(ActorWrapper wrapper, bool autoinit = true) {
            GetComponent<Renderer>().material = ResourceManager.Instance.bloccoControllo;
            hat = new Hat(wrapper.Actor);
            Environment.Instance.PlayModeStarted += hat.Execute;
            hat.PrepareForDetachment += () => { Environment.Instance.PlayModeStarted -= hat.Execute; };
            testo = "Al play mode";
			base.Init (wrapper, autoinit);
        }


        
    }
}