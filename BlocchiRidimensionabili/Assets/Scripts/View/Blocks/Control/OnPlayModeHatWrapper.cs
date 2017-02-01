using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view {
    public class OnPlayModeHatWrapper : HatWrapper {

        // Use this for initialization
        protected override void Start() {
            hat = new Hat();
            Environment.Instance.PlayModeStarted += hat.Execute;
            testo = "Al play mode";
            base.Start();
        }
        
    }
}