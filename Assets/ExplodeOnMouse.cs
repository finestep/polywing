using UnityEngine;


public class ExplodeOnMouse : MonoBehaviour {
    public GameObject terrain;
    public GameObject explo;
    public float radius;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            GameObject e = Instantiate(explo);
            e.transform.position = mousePos;
            e.transform.localScale = new Vector3(radius, radius, radius);
            e.GetComponent<ParticleSystem>().Play();
            Destroy(e, e.GetComponent<ParticleSystem>().duration);

            if (terrain)
                terrain.GetComponent<DeformTerrain>().Deform( new DeformTerrain.Circle(mousePos, radius) );

        }
	}
}
