using UnityEngine;
using System.Collections;

public class BasicProjectile : Weapon {

    public float vel;
    public float inherit;

    public float num;

    public float spread;
    public float posVariance;

    public GameObject proj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	override public void Update () {
        base.Update();
	}

    override public bool Fire()
    {
        if (!base.Fire()) return false;

        for (int i = 0; i < num; i++)
        {
            GameObject p = (GameObject)Instantiate(proj,
                (Vector2)transform.position + 0.2f * (Vector2)transform.right 
                + Random.insideUnitCircle*posVariance, transform.rotation);
            Physics2D.IgnoreCollision(p.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>());

            Rigidbody2D prb = p.GetComponentInParent<Rigidbody2D>();
            if (prb) prb.velocity += (Vector2)transform.right * vel + Random.insideUnitCircle * spread
                                  + GetComponentInParent<Rigidbody2D>().velocity * inherit;
        }
        return true;
    }
}
