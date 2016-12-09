using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CircleTest : MonoBehaviour {
    public float radius;

    public Transform At;
    public Transform Bt;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 C = transform.position;

        Vector2 pos = At.position;
        Vector2 delta = (Vector2)Bt.position - pos;

        Debug.DrawRay(pos, delta);


        for (int i = 0; i < 5; i++)
        {
            Vector3 rvec = Random.onUnitSphere;
            rvec.z = 0;
            rvec = rvec.normalized * radius;
            Debug.DrawRay(C, rvec, Color.gray);
        }



        float quadA = delta.sqrMagnitude;
        float quadB = 2 * ( Vector2.Dot(pos,delta) - Vector2.Dot(C,delta) );
        float quadC = pos.sqrMagnitude - 2 * Vector2.Dot(pos, C)
                        + C.sqrMagnitude - radius*radius;
        float discr = quadB * quadB - 4 * quadA * quadC;

        float alpha1 = (-quadB - Mathf.Sqrt(discr)) / (2 * quadA);
        float alpha2 = (-quadB + Mathf.Sqrt(discr)) / (2 * quadA);


        Vector2 pos1 = pos + delta * alpha1;
        Vector2 pos2 = pos + delta * alpha2;

        if( 0 < alpha1 && alpha1 < 1 ) Debug.DrawLine(transform.position, pos1);
        if (0 < alpha2 && alpha2 < 1) Debug.DrawLine(transform.position, pos2);
    }
}
