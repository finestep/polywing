using UnityEngine;
using System.Collections;

public class ShipControls : MonoBehaviour {

    Rigidbody2D rb;

    public float thrust;
    public float turnSpeed;
    public float maxSpeed;

    public GameObject[] weapons;

    bool thrusting;
    int turn;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        thrusting = false;
        turn = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Vertical") > 0) thrusting = true;
        else thrusting = false;
        if (Input.GetAxis("Horizontal") > 0) turn = -1;
        else if (Input.GetAxis("Horizontal") < 0) turn = 1;
        else turn = 0;

        weapons[0].GetComponent<Weapon>().energy += Time.deltaTime*10;

        if (Input.GetAxis("Fire1")>0)
        {
            weapons[0].GetComponent<Weapon>().Fire();
        }
	}

    void FixedUpdate()
    {
        if (thrusting) rb.velocity += (Vector2)transform.right * thrust * Time.fixedDeltaTime;
        if (turn != 0) rb.rotation += turnSpeed * turn;

        if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
    }
}
