using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;

public class SelectableActor : LaserSelectable {

	public static ActorWrapper selectedActor;


	public override void Select(){
		if (selectedActor != null && selectedActor != this.GetComponent<ActorWrapper> ()) {
			selectedActor.HideBlocks ();
		}
		selectedActor = this.GetComponent<ActorWrapper> ();
		//Debug.Log ("Selected actor " + name);
		selectedActor.ShowBlocks();
	}

}
