using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BloccoCappello : Blocco
{

    public override List<Dente> denti
    {
        get
        {
            var r = new List<Dente>();
            r.Add(dente);
            return r;
        }
    }

    public override string ToString()
    {
        return testo;
    }

    public override void setNext(Blocco candidateNext)
    {
        Debug.Log(testo + ".setNext(" + candidateNext.testo + ")");

        var selectionList = new Dictionary<GameObject, Vector2>();
        candidateNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
        selectionList.Add(candidateNext.gameObject, new Vector2(0, 0));

        selectionList.Keys.ToList().ForEach(k =>
            {
                k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionList[k].x, nextBlockOffsetY - selectionList[k].y, 0);
                k.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, 180);
            });

        var oldNext = next;
        next = candidateNext;

        candidateNext.spazioDente.Receiving = false;
        dente.Receiving = false;
        dente.Receiving = true;
        if (oldNext)
        {
            var blocchi = new List<Blocco>();
            blocchi.Add(next);
            next.linkedBlocks.ForEach(blocchi.Add);
            blocchi.Last().setNext(oldNext);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Return))
            GameObject.Find("Output").GetComponent<UnityEngine.UI.Text>().text = next.EvaluateMe("");
    }


    protected override void Start()
    {
        dente.setNext = setNext;
        dente.unsetNext = unsetNext;
        name = testo;
        campoTesto.text = testo;
        if (lastBlock) evaluateLastBlock();
        nextBlockOffsetX = 2;
        offsetTestoBaseX = 0;
    }
}
