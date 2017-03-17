using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionMenu : MonoBehaviour
{

	public Material[] states;
	private int selectedPage = 0;
	private int variablePage = 6;
	public event System.Action<int> screenChanged;

	public UnityEngine.UI.Text screenText;
	private int[] totalScreenNumber, currentScreen;
	public MenuOption[] options;

	public void UpdateScreenNumber (int currentScreen)
	{
		string newText = (currentScreen + 1) + "/" + totalScreenNumber [selectedPage];
		screenText.text = newText;
	}

	public bool HasNextScreen {
		get { return currentScreen [selectedPage] < (totalScreenNumber [selectedPage] - 1); }
	}

	public bool HasPreviousScreen {
		get { return currentScreen [selectedPage] > 0; }
	}

	public void ChangePage (int page)
	{
		if (page < 0 || page > states.Length)
			throw new System.Exception ("Bad number");
		selectedPage = page;
		this.GetComponent<Renderer> ().material = states [page];
		if (screenChanged != null)
			screenChanged (currentScreen [selectedPage]);
		UpdateOptions (page);
	}

	public void NextScreen ()
	{
		if (currentScreen [selectedPage] < totalScreenNumber [selectedPage]) {
			currentScreen [selectedPage]++;
			if (screenChanged != null)
				screenChanged (currentScreen [selectedPage]);
		}
		UpdateOptions (selectedPage);
	}

	public void PrevScreen ()
	{
		if (currentScreen [selectedPage] > 0) {
			currentScreen [selectedPage]--;
			if (screenChanged != null)
				screenChanged (currentScreen [selectedPage]);
		}
		UpdateOptions (selectedPage);
	}

	private void Start ()
	{
		totalScreenNumber = new int[7];
		currentScreen = new int[7];
		this.screenChanged += UpdateScreenNumber;
		SelectableActor.selectedActorChanged += UpdateActor;

		int i = 0;
		totalScreenNumber.ToList ().ForEach (s => {
			if (i != variablePage)
				totalScreenNumber [i] = ResourceManager.Instance.prototypes [i].Length / options.Length + 1;
			else {
				//First esteem, will then be updated
				totalScreenNumber [i] = model.Environment.Instance.Variables.Count / options.Length + 1;
			}
			i++;
		});

		options.ToList ().ForEach (s => s.Contained = null);

		if (screenChanged != null)
			screenChanged (currentScreen [selectedPage]);
		UpdateOptions (0);
	}

	private void UpdateOptions (int page)
	{
		options.ToList ().ForEach (s => s.Contained = null);
		if (page != variablePage) {
			int i = currentScreen [page] * 9;
			options.ToList ().ForEach (s => {
				if (i < ResourceManager.Instance.prototypes [page].Length)
					s.Contained = ResourceManager.Instance.prototypes [page] [i++];
			});
		} else {
			var variables = new List<ResourceManager.MenuVoiceVariablePrefab> ();
			model.Environment.Instance.Variables.ForEach (s => variables.Add(new ResourceManager.MenuVoiceVariablePrefab(s)));
			var actor = SelectableActor.selectedActor;
			if (actor != null)
				actor.Actor.variables.ToList().ForEach (s => variables.Add(new ResourceManager.MenuVoiceVariablePrefab(s)));
            int i = currentScreen[page] * 9;
            options.ToList().ForEach(s => {
                if (i < variables.Count)
                    s.Contained = variables[i++];
            });
        }
	}
	private void UpdateActor()
	{
		UpdateOptions (selectedPage);
	}

    

    private void OnDestroy ()
	{
		this.screenChanged -= UpdateScreenNumber;
		SelectableActor.selectedActorChanged -= UpdateActor;
	}
}
