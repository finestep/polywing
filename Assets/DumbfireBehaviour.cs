using UnityEngine;
using System.Collections;

public class DumbfireBehaviour : MonoBehaviour {

    public GameObject explo;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject e = (GameObject)Instantiate(explo, transform.position, Quaternion.identity);
        e.GetComponent<BasicExplosionBehaviour>().radius = 0.5f;
        Destroy(e, 3);
        Destroy(gameObject);
    }
}
