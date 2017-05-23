using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HealthbarBehaviour : MonoBehaviour {
    public Color fgCol;
    public Color bgCol;

    public float val;
    public float maxVal;


    public float size;
    public float width;

    LineRenderer FG;
    LineRenderer BG;


    bool init = false;
    
    // Use this for initialization
    void Start () {
        FG = transform.Find("FG").GetComponent<LineRenderer>();
        BG = transform.Find("BG").GetComponent<LineRenderer>();

        FG.SetPosition(0, new Vector2( -size / 2, 0 ));
        BG.SetPosition(1, new Vector2( size / 2, 0 ));

        FG.useWorldSpace = false;
        BG.useWorldSpace = false;

        fgCol.a = 1;
        bgCol.a = 1;


        init = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!init) Start();



        FG.startColor = fgCol;
        FG.endColor = fgCol;
        BG.startColor = bgCol;
        BG.endColor = bgCol;


        FG.startWidth = width;
        FG.endWidth = width;
        BG.startWidth = width;
        BG.endWidth = width;

        float ratio = val / maxVal;
        Vector2 pos = new Vector2(-size / 2 + size * ratio, 0);
        FG.SetPosition(1, pos );
        BG.SetPosition(0, pos);
    }
}
