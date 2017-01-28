using System.Collections.Generic;
using System;
using UnityEngine;

public class Actor {

	private Vector3 position, rotation;
	private float scale, volume;
	private string message;
	private IList<Hat> hats;
	private IList<Block> blocks;
	public event Action ShowedMessage;
	public event Action HiddenMessage;

	public Actor (Vector3 position, Vector3 rotation, float scale, float volume, string message)
	{
		hats = new List<Hat> ();
		blocks = new List<Block> ();
	}

	public Actor ()
		:this( new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 1, 75, ""){

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

	public void ShowMessage(){
		if (ShowedMessage != null)
			ShowedMessage();
	}

	public void HideMessage(){
		if (HiddenMessage != null)
			HiddenMessage ();
	}

	public void AddHat(Hat hat) {
		hats.Add (hat);
	}
	public void RemoveHat(Hat hat) {
		hats.Remove (hat);
	}
	public void AddBlock(Block block) {
		blocks.Add (block);
	}
	public void RemoveBlock(Block block) {
		blocks.Remove (block);
	}
}
