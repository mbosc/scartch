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
        get { return currentScreen[selectedPage] < (totalScreenNumber[selectedPage] - 1); }
    }
    public bool HasPreviousScreen
    {
        get { return currentScreen[selectedPage] > 0; }
    }

    public void ChangePage(int page)
    {
        if (page < 0 || page > states.Length)
            throw new System.Exception("Bad number");
        selectedPage = page;
        this.GetComponent<Renderer>().material = states[page];
        if (screenChanged != null)
            screenChanged(currentScreen[selectedPage]);
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
        UpdateOptions(selectedPage);
    }
    public void PrevScreen()
    {
        if (currentScreen[selectedPage] > 0)
        {
            currentScreen[selectedPage]--;
            if (screenChanged != null)
                screenChanged(currentScreen[selectedPage]);
        }
        UpdateOptions(selectedPage);
    }

    private void Start()
    {
        totalScreenNumber = new int[7];
        currentScreen = new int[7];
        this.screenChanged += UpdateScreenNumber;

        int i = 0;
        totalScreenNumber.ToList().ForEach(s => { totalScreenNumber[i] = ResourceManager.Instance.prototypes[i].Length / options.Length + 1; i++; });

        options.ToList().ForEach(s => s.Contained = null);

        if (screenChanged != null)
                screenChanged(currentScreen[selectedPage]);
        UpdateOptions(0);
    }

    private void UpdateOptions(int page)
    {
        options.ToList().ForEach(s => s.Contained = null);
        int i = currentScreen[page] * 9;
        options.ToList().ForEach(s => {
            if (i < ResourceManager.Instance.prototypes[page].Length)
                s.Contained = ResourceManager.Instance.prototypes[page][i++];
        });
    }

    private void OnDestroy()
    {
        this.screenChanged -= UpdateScreenNumber;
    }
}
