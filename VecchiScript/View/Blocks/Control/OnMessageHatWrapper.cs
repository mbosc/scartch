using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view {
    public class OnMessageHatWrapper : HatWrapper {

        // Use this for initialization
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
            GetComponent<Renderer>().material = ResourceManager.Instance.bloccoControllo;
            hat = new OnMessageHat(wrapper.Actor);
            hat.PrepareForDetachment += () => { Environment.Instance.PlayModeStarted -= hat.Execute; };
            testo = "Al play mode";
            base.Init(wrapper, autoinit);
        }

        private class OnMessageHat : Hat
        {

        }


        
    }
}