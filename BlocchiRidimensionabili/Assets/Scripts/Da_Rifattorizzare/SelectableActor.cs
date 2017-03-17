using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;

public class SelectableActor : LaserSelectable {

	public static ActorWrapper selectedActor;
	public static Action selectedActorChanged;

	void Start(){
		selectedActorChanged += showActorBlocks;
	}

	void OnDestroy(){
		selectedActorChanged -= showActorBlocks;
	}

	public override void Select(){
		if (selectedActor != null && selectedActor != this.GetComponent<ActorWrapper> ()) {
			selectedActor.HideBlocks ();
		}
		selectedActor = this.GetComponent<ActorWrapper> ();
		if (selectedActorChanged != null)
			selectedActorChanged ();
	}

	private void showActorBlocks(){
		selectedActor.ShowBlocks();
	}

}
