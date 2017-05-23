using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRotateWithParent : MonoBehaviour {


    public Vector3 offset;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.identity;

        transform.position = transform.root.position + offset;
    }
}
