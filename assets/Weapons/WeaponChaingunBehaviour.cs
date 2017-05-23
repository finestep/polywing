using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChaingunBehaviour : BasicProjectileWeapon {

    public float windUp;
    public float maxFirerate;
    public float windDown;

    float baseRate;
    bool fired = true;

	// Use this for initialization
	void Start () {
        baseRate = firerate;
	}

    public override void Update()
    {
        base.Update();
        if (reload < 0 && !fired) firerate += windDown;
        if (firerate > baseRate) firerate = baseRate;
        fired = false;
    }

    override public bool Fire()
    {
        if ( base.Fire() )
        {

            fired = true;
            firerate -= windUp;
            if (firerate < maxFirerate) firerate = maxFirerate;
            return true;
        }
        return false;
    }
}
