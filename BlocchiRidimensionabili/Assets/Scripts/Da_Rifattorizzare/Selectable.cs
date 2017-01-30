using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selectable : MonoBehaviour {

  //  private Selector selector;
  //  private Dictionary<GameObject, Vector2> selectionList;

  //  // Use this for initialization
  //  void Start() {
  //      selector = FindObjectOfType<Selector>();
  //  }

  //  public List<Dente> denti
  //  {
  //      get
  //      {
  //          List<Dente> risposta = new List<Dente>();
  //          selectionList.Keys.ToList().ForEach(k => k.GetComponentsInChildren<Dente>().ToList().ForEach(risposta.Add));
  //          return risposta;
  //      }
  //  }
  //  public List<SpazioDente> spaziDenti
  //  {
  //      get
  //      {
  //          List<SpazioDente> risposta = new List<SpazioDente>();
  //          selectionList.Keys.ToList().ForEach(k => k.GetComponentsInChildren<SpazioDente>().ToList().ForEach(risposta.Add));
  //          return risposta;
  //      }
  //  }

  //  void OnMouseOver() {
  //      selector.hovered = this;
  //      selectionList = new Dictionary<GameObject, Vector2>();
		//var blocco = GetComponent<BlockWrapper>();
		//if (blocco) {
		//	blocco.linkedBlocks.ForEach (
		//		s => 
		//		{
		//			selectionList.Add (s.gameObject, new Vector2 (gameObject.transform.position.x - s.gameObject.transform.position.x, gameObject.transform.position.y - s.gameObject.transform.position.y));
		//			s.slotVariabili.ForEach(z => 
		//			{ 
		//				if (z.variabile)
		//					selectionList.Add (z.variabile.gameObject, new Vector2 (gameObject.transform.position.x - z.variabile.gameObject.transform.position.x,gameObject.transform.position.y - z.variabile.gameObject.transform.position.y));
		//			});
		//		});
		//	blocco.slotVariabili.ForEach (s => 
		//		{ if (s.variabile)
		//			selectionList.Add (s.variabile.gameObject, 
		//				new Vector2 (gameObject.transform.position.x - s.variabile.gameObject.transform.position.x,
		//					gameObject.transform.position.y - s.variabile.gameObject.transform.position.y));});
		//}
  //      selectionList.Add(gameObject, new Vector2(0, 0));
  //  }
  //  void OnMouseExit() {

  //      if (selector.hovered == this)
  //          selector.hovered = null;
  //  }

  //  internal void move(Vector3 input)
  //  {
  //      selectionList.Keys.ToList().ForEach(k => k.transform.position = new Vector3(input.x - selectionList[k].x, input.y - selectionList[k].y, input.z));
  //  } 

  //  internal void move(Vector2 input)
  //  {
  //      move(new Vector3(input.x, input.y, 0));
  //  }
}
