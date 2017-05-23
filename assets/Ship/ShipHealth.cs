using UnityEngine;
using System.Collections;


public class ShipHealth : MonoBehaviour {

    HealthbarBehaviour bar;


    public enum DMGTYPE
    {
        TERRAIN = 0,
        BALLISTIC,
        EXPLOSIVE,
        FIRE,

        COUNT
    }

    

    public float maxHP;

    float HP;

    public float fireTreshhold;

    public float fireDamage;

    public bool onFire;

    float whiteFlash;

    PolyshipMesh pm;

    Color baseCol;

    public GameObject explosionPrefab;

    public GameObject shockwave;

    GameObject explosion;


	// Use this for initialization
	void Start () {
        pm = GetComponent<PolyshipMesh>();

        HP = maxHP;
        onFire = false;

        bar = transform.FindChild("Healthbar").GetComponent<HealthbarBehaviour>();

        bar.maxVal = maxHP;

        onFire = false;

        explosion = null;
	}

    // Update is called once per frame
    void Update()
    {


        bar.val = HP;
        if (bar.val < 0) bar.val = 0;

        //if (onFire)
        //    HP -= fireDamage*Time.deltaTime;

        if (HP < 0 && !onFire)
        {
            onFire = true;
            explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            pm.FlashWhite(3);
            Destroy(gameObject, 3f);
            Destroy(explosion, 4f);
        }

        if (explosion != null)
        {
            explosion.transform.position = transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (explosion != null)
            explosion.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
    }

    private void OnDestroy()
    {
        if (onFire)
        {
            Instantiate(shockwave, transform.position, Quaternion.identity);
            explosion.transform.position = transform.position;
            explosion.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void TakeDamage(float dmg, DMGTYPE t)
    {
        //Debug.Log("Took " + dmg);
        //Debug.Assert(dmg > 0);
        HP -= dmg;
        float flashTime = dmg / 30;
        flashTime = Mathf.Clamp(flashTime, 0.08f, 0.4f);
        pm.FlashWhite( flashTime );
    }

}
