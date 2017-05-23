using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PolyshipMesh : MonoBehaviour {

    public Color shipCol;

    public float faceAngle;

    MeshFilter mf;

    public List<int> triangles;
    List<Vector3> verts;
    public Color[] colors;
    public Color[] shades;

    float flashTime;

    bool init = false;

    public void FlashWhite( float duration )
    {
        flashTime += duration;
    }

    // Use this for initialization
    void Start () {
        mf = GetComponent<MeshFilter>();
        if (!mf)
            mf = gameObject.AddComponent<MeshFilter>();
        mf.sharedMesh = new Mesh();
        mf.sharedMesh.Clear();
        GetComponent<MeshRenderer>().material.Equals(null);
        
        triangles = new List<int>();

        verts = new List<Vector3>();

        verts.Add(new Vector3(0, 0.2f));
        verts.Add(new Vector3(-0.15f, -0.15f));
        verts.Add(new Vector3(0, -0.1f));

        verts.Add(new Vector3(0, 0.2f));
        verts.Add(new Vector3(0.15f, -0.15f));
        verts.Add(new Vector3(0, -0.1f));

        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(1);

        triangles.Add(3);
        triangles.Add(4);
        triangles.Add(5);

        mf.sharedMesh.SetVertices(verts);

        mf.sharedMesh.SetTriangles(triangles, 0);

        colors = new Color[6];

        shades = new Color[2];

        shades[0] = shipCol;

        shades[0].r += 0.2f;
        shades[0].g += 0.2f;
        shades[0].b += 0.2f;

        if (shades[0].r > 1.0f) shades[0].r = 1.0f;
        if (shades[0].g > 1.0f) shades[0].g = 1.0f;
        if (shades[0].b > 1.0f) shades[0].b = 1.0f;

        shades[1] = shipCol;

        shades[1] *= 0.5f;

        shades[0].a = 1.0f;
        shades[1].a = 1.0f;

        flashTime = 0;

        init = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (!init) Start();

        Color c1 = Color.white, c2 = Color.white;

        if (flashTime > 0.0f)
        {
            flashTime -= Time.deltaTime;
        }
        else
        {

            float ang = transform.rotation.eulerAngles.z;

            float alpha = Mathf.Abs(Mathf.DeltaAngle(-faceAngle, ang) / 180.0f);

            c1 = shades[1] * alpha + shades[0] * (1.0f - alpha);

            alpha = Mathf.Abs(Mathf.DeltaAngle(faceAngle, ang) / 180.0f);

            c2 = shades[1] * alpha + shades[0] * (1.0f - alpha);

            c2 *= 0.8f;
            c2.a = 1;
        }

        for (int i = 0; i < 3; ++i) colors[i] = c1;
        for (int i = 3; i < 6; ++i) colors[i] = c2;

        mf.sharedMesh.SetColors(new List<Color>(colors));
    }

}
