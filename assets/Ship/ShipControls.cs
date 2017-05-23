using UnityEngine;
using System.Collections;

public class ShipControls : MonoBehaviour {

    Rigidbody2D rb;

    public int id;

    public float thrust;
    public float turnSpeed;
    public float maxSpeed;

    public float power;

    Weapon[] weapons;
    bool thrusting;
    float turn;

    int selectedWeapon;

    ParticleSystem.EmissionModule em;

    HealthbarBehaviour reloadBar;

    public GameObject floatText;

    GameObject text;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        thrusting = false;
        em = GetComponentInChildren<ParticleSystem>().emission;
        em.enabled = false;
        turn = 0;

        weapons = gameObject.GetComponentsInChildren<Weapon>();

        selectedWeapon = 0;

        reloadBar = transform.FindChild("Reloadbar").GetComponent<HealthbarBehaviour>();

        text = null;
    }
	
	// Update is called once per frame
	void Update () {

        turn = Input.GetAxis("Turning_" + id);

        if (Input.GetButton("Thrust_" + id)) thrusting = true;
        else thrusting = false;
        


        if (Input.GetButtonUp("NextWeapon_" + id)) selectedWeapon++;
        if (Input.GetButtonUp("PrevWeapon_" + id)) selectedWeapon--;

        if (selectedWeapon >= weapons.Length) selectedWeapon = 0;
        if (selectedWeapon < 0) selectedWeapon = weapons.Length - 1;

        if (Input.GetButtonUp("NextWeapon_" + id) || Input.GetButtonUp("PrevWeapon_" + id))
        {
            if (text != null) Destroy(text);
            text = Instantiate(floatText, transform);
            text.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            text.GetComponent<TextMesh>().text = weapons[selectedWeapon].printName;
            Destroy(text, 0.6f);
        }

        int rechargeCount = 0;
        foreach(Weapon wep in weapons) {
            if (wep.energy < wep.maxEnergy) rechargeCount++;
        }


        bool currWepCharged = weapons[selectedWeapon].energy >= weapons[selectedWeapon].maxEnergy;
        float recharge;

        if (rechargeCount > 0)
        {
            foreach (Weapon wep in weapons)
            {

                if (currWepCharged)
                {
                    recharge = power / rechargeCount;
                }
                else {
                    if (ReferenceEquals(wep, weapons[selectedWeapon]) ) recharge = power * 0.75f;
                    else recharge = power * 0.25f / rechargeCount;
                }
                 wep.Recharge(recharge * Time.deltaTime);

            }
        }

        reloadBar.val = weapons[selectedWeapon].energy;
        reloadBar.maxVal = weapons[selectedWeapon].maxEnergy;
        
        em.enabled = thrusting;

        if (Input.GetButton("Fire_" + id) )
        {
            weapons[selectedWeapon].Fire();
        }

        if (GetComponent<ShipHealth>().onFire) thrust *= 0.994f;
        if (thrust < 0.1f) thrusting = false;

    }

    void FixedUpdate()
    {
        if (thrusting) rb.velocity += (Vector2)transform.up * thrust * Time.fixedDeltaTime;
        if (turn != 0) rb.rotation += turnSpeed * turn;

        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}
