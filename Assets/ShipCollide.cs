using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ShipHealth))]
public class ShipCollide : MonoBehaviour {

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Velocity on impact: " + collision.relativeVelocity.magnitude);
        if (collision.gameObject.CompareTag("Terrain"))
        {


            if (collision.relativeVelocity.magnitude < 4) return;

            if (Vector2.Dot(collision.relativeVelocity, collision.contacts[0].normal) < 0)
            {
                rb.AddForce(-collision.relativeVelocity.magnitude * collision.contacts[0].normal, ForceMode2D.Impulse);

                float dmg = collision.relativeVelocity.magnitude;

                dmg = Mathf.Clamp(dmg, 0, 30);

                GetComponent<ShipHealth>().TakeDamage(dmg, ShipHealth.DMGTYPE.TERRAIN);
            }
        }
    }


}
