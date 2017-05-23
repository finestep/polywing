using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeepShipsOnCamera : MonoBehaviour {

    public float minSize;

    public float margin;

    GameObject[] ships;

    Camera c;

    public float maxSpeed;


	// Use this for initialization
	void Start () {
        ships = GameObject.FindGameObjectsWithTag("Ship");
        c = GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {

        int shipsAlive = 0;

        float maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;
        float minX = float.PositiveInfinity, minY = float.PositiveInfinity;
        foreach (GameObject ship in ships)
        {
            if (ship)
            {
                shipsAlive++;

                Vector2 spos = ship.transform.position;
                if (spos.x * margin > maxX)
                    maxX = spos.x * margin;
                if (spos.y * margin > maxY)
                    maxY = spos.y * margin;

                if (spos.x * margin < minX)
                    minX = spos.x * margin;
                if (spos.y * margin < minY)
                    minY = spos.y * margin;
            }
        }

        if (shipsAlive == 0) return;
        if(shipsAlive == 1)
        {
            c.orthographicSize += 0.04f;
            return;
        }

        Vector3 center = new Vector3((minX + maxX)/margin / 2, (minY + maxY) / margin / 2, -10);
        c.transform.position = Vector3.MoveTowards(c.transform.position, center, maxSpeed);

        float newSize = Mathf.Max(minSize,
            Mathf.Abs( (center.x-maxX)/c.aspect),
            Mathf.Abs((center.x-minX) / c.aspect),
            Mathf.Abs(center.y-maxY),
            Mathf.Abs(center.y-minY)
        );


        c.orthographicSize = Mathf.MoveTowards(c.orthographicSize, newSize, maxSpeed);

        c.ResetCullingMatrix();
        c.ResetWorldToCameraMatrix();
    }
}
