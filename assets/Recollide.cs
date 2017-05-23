using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recollide : MonoBehaviour {

    public GameObject parent; //weapon, not ship
    public float delay;
    
	void Start () {
        Invoke("Execute", delay);
	}
	
	void Execute()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), parent.GetComponentInParent<Collider2D>(),false);
    }
}
