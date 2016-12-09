using UnityEngine;
using System.Collections;

public class ShipHealth : MonoBehaviour {

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

    bool onFire;

    float whiteFlash;

    SpriteRenderer sr;

    Color baseCol;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();

        HP = maxHP;
        onFire = false;
        whiteFlash = -1;
        baseCol = sr.color;
	}

    Vector2 offset = new Vector2(-0.5f, 0.5f);
    Vector2 hpVec = new Vector2();
    Vector2 dmgVec = new Vector2();

    // Update is called once per frame
    void Update () {

        hpVec.x = HP / maxHP;
        dmgVec.x = 1 - HP / maxHP;

        Debug.DrawRay((Vector2)transform.position + offset, hpVec,Color.green);
        Debug.DrawRay((Vector2)transform.position + offset + hpVec, dmgVec, Color.red);

        if (onFire)
            HP -= fireDamage*Time.deltaTime;

        if (HP < 0)
            Destroy(gameObject);

        if(whiteFlash>0)
        {
            whiteFlash -= Time.deltaTime;
            sr.color = Color.white;
        } else if(whiteFlash!=-1)
        {
            whiteFlash = -1;
            sr.color = baseCol;
        }
	}

    public void TakeDamage(float dmg, DMGTYPE t)
    {
        //Debug.Log("Took " + dmg);
        //Debug.Assert(dmg > 0);
        HP -= dmg;
        float flashTime = dmg / 30;
        flashTime = Mathf.Clamp(flashTime, 0.08f, 0.4f);
        whiteFlash = flashTime;
    }

}
