using UnityEngine;
using System.Collections;

public class BasicExplosionBehaviour : MonoBehaviour {

    public float radius;
    public float force;
    public float damage;

    public float forceAge;
    public float damageAge;

    public int shrapNum;
    public GameObject shrap;

    int age;

	// Use this for initialization
	void Start () {

        transform.localScale = new Vector3(radius, radius, radius);

        FindObjectOfType<DeformTerrain>().Deform(new DeformTerrain.Circle(transform.position, radius));

        for(int i = 0; i<shrapNum;i++)
        {
            Vector3 v = Random.insideUnitCircle.normalized;
            GameObject s = Instantiate(shrap,
                 transform.position + v * Random.Range(0, radius),
                transform.rotation);
            s.GetComponent<ShrapnelBehaviour>().life = 4 + Random.Range(0, 5);
            s.GetComponent<Rigidbody2D>().AddForce(v * Random.Range(0.08f,0.13f),ForceMode2D.Impulse);
        }


        GetComponentInChildren<ParticleSystem>().Play();
        Destroy(gameObject, GetComponentInChildren<ParticleSystem>().duration);
        age = 0;
	}

    private void Update()
    {
        age++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Vector2 delta = collision.transform.position - transform.position;
            float dist = 1+delta.magnitude;
            if (age < forceAge)
                collision.attachedRigidbody.AddForce(delta.normalized * force / dist / dist, ForceMode2D.Impulse);
            if (age < damageAge)
                collision.gameObject.GetComponent<ShipHealth>().TakeDamage(damage / dist, ShipHealth.DMGTYPE.EXPLOSIVE);
        }
    }
}
