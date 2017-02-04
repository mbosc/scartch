using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMessage : MonoBehaviour {

	private string text;
	public UnityEngine.UI.Image start, body, end;
	public UnityEngine.UI.Text inText;

	public string Text {
		get { return text; }
		set {
			text = value;
			inText.text = value;
			Extend ();
		}
	}

	private void Extend(){
		var lunghezza = text.Length;
		if (text.Length < 5)
			lunghezza = 5;
		var extension = (lunghezza - 2) / 3.0f * 5;
		var displacement = -21.59f + (extension) * 0.5f;
		var assDisplacement = -21.115f + extension;
		Debug.Log (extension + ", " + displacement +"-> "+ assDisplacement);
		//body.rectTransform.rect.width = extension;
		//body.rectTransform.rect.position.x = -19.09f + extension/5 * 0.5f;
		body.rectTransform.sizeDelta = new Vector2(extension, body.rectTransform.sizeDelta.y);
		body.rectTransform.localPosition = new Vector3(displacement, body.rectTransform.localPosition.y, body.rectTransform.localPosition.z);

		end.rectTransform.localPosition = new Vector3(assDisplacement, end.rectTransform.localPosition.y, end.rectTransform.localPosition.z);
	}
}
