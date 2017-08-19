using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxiliaryTimer : MonoBehaviour {
    
    public void ExecuteDelayed(float delay, System.Action action)
    {
        StartCoroutine(WaitAndDo(delay, action));
    }

    private IEnumerator WaitAndDo(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
