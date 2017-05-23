using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float maxEnergy;
    public float energyPerShot;
    public float firerate;
    public bool clip;

    public string printName;

    public float energy;

    protected float reload;

    protected bool clipLoaded;

	// Use this for initialization
	void Start () {
        energy = 0;
        reload = 0;
        clipLoaded = false;
	}
	
	// Update is called once per frame
	public virtual void Update () {
        if (reload > 0) reload -= Time.deltaTime;
	}

    public virtual void Recharge(float re)
    {
        if (!clip || !clipLoaded) energy += re;

        if (energy > maxEnergy)
        {
            energy = maxEnergy;
            clipLoaded = true;
        }
    }

    public virtual bool Fire()
    {
        if (reload > 0 || (!clip && energy < energyPerShot ) || (clip && !clipLoaded ) ) return false;

        energy -= energyPerShot;
        reload = firerate;

        if(energy<=0)
        {
            energy = 0;
            clipLoaded = false;
        }

        return true;
    }
}
