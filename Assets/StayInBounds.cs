using UnityEngine;
using System.Collections;

public class StayInBounds : MonoBehaviour {

    Rigidbody2D rb;

    public float minX, maxX, minY, maxY;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {

        if (rb.position.x < minX)
            rb.AddForce(new Vector2(minX-rb.position.x-rb.velocity.x*0.1f,0)*100, ForceMode2D.Force);
        if (rb.position.x > maxX)
            rb.AddForce(new Vector2(maxX-rb.position.x - rb.velocity.x * 0.1f, 0) * 100, ForceMode2D.Force);

        if (rb.position.y < minY)
            rb.AddForce(new Vector2(0,minY - rb.position.y - rb.velocity.y * 0.1f) * 100, ForceMode2D.Force);
        if (rb.position.y > maxY)
            rb.AddForce(new Vector2(0,maxY - rb.position.y - rb.velocity.y * 0.1f) * 100, ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
