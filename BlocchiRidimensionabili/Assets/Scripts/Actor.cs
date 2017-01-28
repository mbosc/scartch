using System.Collections;
using System;
using UnityEngine;

public class Actor {

	private Vector3 position, rotation;
	private float scale, volume;
	private string message;

	public Actor ()
	{
		position = new Vector3 (0, 0, 0);
		rotation = new Vector3 (0, 0, 0);
		scale = 1;
		volume = 75;
		message = "";
	}

	public Vector3 Position {
		get {
			return position;
		}
		set {
			// TODO verifica che non superi i margini dell'Environment
			position = value;
		}
	}

	public Vector3 Rotation {
		get {
			return rotation;
		}
		set {
			rotation = value;
		}
	}

	public float Scale {
		get {
			return scale;
		}
		set {
			
			if (scale < 0)
				// e poi questo chi lo cattura?
				throw new ArgumentException ("Scala negativa");
			// TODO verifica limiti del sistema
			scale = value;
		}
	}

	public float Volume {
		get {
			return volume;
		}
		set {
			if (volume < 0 || volume > 100)
				throw new ArgumentException ("Volume must be in range [0, 100]");
			volume = value;
		}
	}

	public string Message {
		get {
			return message;
		}
		set {
			if (message == null)
				throw new ArgumentException ("Message is null");
			message = value;
		}
	}

	public event Action ShowedMessage;
	public event Action HiddenMessage;

	public void ShowMessage(){
		if (ShowedMessage != null)
			ShowedMessage();
	}

	public void HideMessage(){
		if (HiddenMessage != null)
			HiddenMessage ();
	}
}
