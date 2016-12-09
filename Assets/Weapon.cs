using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float maxEnergy;
    public float energyPerShot;
    public float firerate;

    public float energy;

    float reload;

	// Use this for initialization
	void Start () {
        energy = 0;
        reload = 0;
	}
	
	// Update is called once per frame
	public virtual void Update () {
        if (reload > 0) reload -= Time.deltaTime;
        if (energy > maxEnergy) energy = maxEnergy;
	}

    public virtual bool Fire()
    {
        if (energy < energyPerShot || reload > 0) return false;

        energy -= energyPerShot;
        reload = firerate;

        return true;
    }
}
