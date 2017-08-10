using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAudioSource : MonoBehaviour {

    public static DebugAudioSource instance;

    // Use this for initialization
    void Start() {
        instance = this;
    }

    public void PlayDebug()
    {
        this.GetComponent<AudioSource>().Play();
    }
    public void PlayDebug(AudioClip a)
    {
        this.GetComponent<AudioSource>().clip = a;
        PlayDebug();
    }
}
