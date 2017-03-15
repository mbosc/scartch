using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionMenu : MonoBehaviour {

    public Material[] states;
    private int selectedPage = 0;
    public event System.Action<int> screenChanged;
    public UnityEngine.UI.Text screenText;
    private int[] totalScreenNumber, currentScreen;
    public MenuOption[] options;

    public void UpdateScreenNumber(int currentScreen)
    {
        string newText = (currentScreen + 1) + "/" + totalScreenNumber[selectedPage];
        screenText.text = newText;
    }

    public bool HasNextScreen
    {
        get { return true; }
    }
    public bool HasPreviousScreen
    {
        get { return true; }
    }

    public void ChangePage(int page)
    {
        if (page < 0 || page > states.Length)
            throw new System.Exception("Bad number");
        selectedPage = page;
        this.GetComponent<Renderer>().material = states[page];
        UpdateOptions(page);
    }
    public void NextScreen()
    {
        if (currentScreen[selectedPage] < totalScreenNumber[selectedPage])
        {
            currentScreen[selectedPage]++;
            if (screenChanged != null)
                screenChanged(currentScreen[selectedPage]);
        }
    }
    public void PrevScreen()
    {
        if (currentScreen[selectedPage] > 0)
        {
            currentScreen[selectedPage]--;
            if (screenChanged != null)
                screenChanged(currentScreen[selectedPage]);
        }
    }

    private void Start()
    {
        totalScreenNumber = new int[7];
        currentScreen = new int[7];
        this.screenChanged += UpdateScreenNumber;
        

        options.ToList().ForEach(s => s.Contained = null);

        if (screenChanged != null)
                screenChanged(currentScreen[selectedPage]);

    }

    private void UpdateOptions(int obj)
    {
        if (obj == 0)
        {
            for (int i = 0; i < ResourceManager.Instance.prototypes.Length; i++)
            {
                options[i].Contained = ResourceManager.Instance.prototypes[i];
            }
        } else
        {
            options.ToList().ForEach(s => s.Contained = null);
        }
    }

    private void OnDestroy()
    {
        this.screenChanged -= UpdateScreenNumber;
     
    }
}
