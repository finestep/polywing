using UnityEngine;
using System.Collections;

public class BasicProjectileWeapon : Weapon {

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
            Vector3 v = Random.insideUnitCircle;

            GameObject p = (GameObject)Instantiate(proj,
                transform.position + 0.3f * transform.up 
                + v*posVariance, transform.rotation);
            Physics2D.IgnoreCollision(p.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>());
            p.AddComponent<Recollide>();
            p.GetComponent<Recollide>().parent = gameObject;
            p.GetComponent<Recollide>().delay = 0.2f;
            Rigidbody2D prb = p.GetComponentInParent<Rigidbody2D>();
            if (prb) prb.velocity += (Vector2)(transform.up * vel + v * spread
                                  + (Vector3)(GetComponentInParent<Rigidbody2D>().velocity) * inherit);
            Rigidbody2D srb = GetComponentInParent<Rigidbody2D>();
            if (srb)
                srb.AddForce(prb.velocity * -prb.mass,ForceMode2D.Impulse);
        }
        return true;
    }
}
