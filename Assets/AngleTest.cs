using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AngleTest : MonoBehaviour {

    bool init;

    public Transform A;
    public Transform B;

    // Use this for initialization
    void Start() {
        A = GetComponentsInChildren<Transform>()[1];
        B = GetComponentsInChildren<Transform>()[2];

        init = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (!init) Start();

        Vector2 p0 = transform.position;
        Vector2 p1 = A.position;
        Vector2 p2 = B.position;

        Vector2 v2 = p1 - p0;
        Vector2 v1 = p2 - p0;

        float ang = Mathf.Atan2(v2.y, v2.x) - Mathf.Atan2(v1.y, v1.x);

        if (ang > Mathf.PI*2) ang -= Mathf.PI*2;
        if (ang < 0) ang += Mathf.PI * 2;

        print(ang*Mathf.Rad2Deg);
    }



    void OnDrawGizmos()
    {



        Gizmos.DrawLine(transform.position, A.position);
        Gizmos.DrawLine(transform.position, B.position);


    }
}
