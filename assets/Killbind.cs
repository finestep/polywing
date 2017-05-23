using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbind : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K)) GetComponent<ShipHealth>().TakeDamage(33333, ShipHealth.DMGTYPE.EXPLOSIVE);
	}
}
