using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDemo : MonoBehaviour {

    public View.HatViewer block1;
    public View.BlockViewer block2;
    
	// Use this for initialization
	void Start () {
        //block1.Type = Scripting.ScriptingType.look;
        //block1.Text = "hello block to you";
        //block1.Deleted += DestroySound;
        //block2.Type = Scripting.ScriptingType.movement;
        //block2.Text = "welo to you, sir";
        //block2.Tested += TestSound;
        //block2.Deleted += DestroySound;
    }

    public AudioClip destroy, test;
    public void DestroySound(object src, System.EventArgs args)
    {
        DebugAudioSource.instance.PlayDebug(destroy);
    }
	public void TestSound()
    {
        DebugAudioSource.instance.PlayDebug(test);
    }
}
