using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveBehaviour : MonoBehaviour {


    SpriteRenderer sr;
    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.localScale = transform.localScale * 1.04f;
        sr.color = sr.color * 0.965f;
        if (sr.color.a < 0.01f) Destroy(gameObject);
	}
}
