using UnityEngine;
using System.Collections;

public class ShipAirdrag : MonoBehaviour {

    public float dragCoeff;

    Rigidbody2D rb;
    

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        float dot = Vector2.Dot(rb.velocity.normalized, transform.right);
        float drag = (2-(dot + 1) )/ 2;

        rb.AddForce(-rb.velocity * dragCoeff * drag);
	}
}
