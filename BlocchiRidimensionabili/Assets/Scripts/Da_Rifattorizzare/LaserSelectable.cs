using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserSelectable : MonoBehaviour
{

    public abstract void SelectA();
    public virtual void SelectB() { }
}
