using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrapnelBehaviour : MonoBehaviour {

    LineRenderer lr;
    Rigidbody2D rb;

    Vector2 origin;

    public float dmg;
    public float life;

    public float maxLen;
    public float velScale;


    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();

        origin = rb.position;
        Destroy(gameObject, life);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnRenderObject()
    {
        Vector3 pos = rb.position;
        pos.z = 1;
        lr.SetPosition(0, pos);
        Vector2 v = rb.velocity * velScale;
        float currMaxLen = Mathf.Min(maxLen, (rb.position - origin).magnitude);
        if (v.magnitude > currMaxLen) v = v.normalized * currMaxLen;
        pos = rb.position - v;
        pos.z = 1;
        lr.SetPosition(1, pos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ship")
        {
            collision.collider.gameObject.GetComponent<ShipHealth>().TakeDamage(dmg, ShipHealth.DMGTYPE.BALLISTIC);
            if(collision.collider.gameObject.GetComponent<Rigidbody2D>())
                collision.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(rb.velocity * rb.mass, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        
        if (rb && rb.velocity.magnitude < 2f) Destroy(gameObject);
    }
}
