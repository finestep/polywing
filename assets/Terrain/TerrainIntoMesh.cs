using UnityEngine;
using System.Collections.Generic;
using System;

using PolyUtil;

[ExecuteInEditMode]
[RequireComponent(typeof(TerrainData),typeof(MeshFilter) )]
public class TerrainIntoMesh : MonoBehaviour {

    MeshFilter mf;
    TerrainData td;

    public List<int>[] triangles;
    List<Vector3> verts;
    Color[] colors;
    LinkVert[] poly;

    bool init;

	// Use this for initialization
	void Start () {
        
        td = GetComponent<TerrainData>();
        mf = GetComponent<MeshFilter>();
        if(!mf)
            mf = gameObject.AddComponent<MeshFilter>();
        mf.sharedMesh = new Mesh();
        mf.sharedMesh.Clear();
        mf.sharedMesh.subMeshCount = 2;

        triangles = new List<int>[GetComponent<MeshRenderer>().sharedMaterials.Length];
        for (int i = 0; i < triangles.Length; i++)
            triangles[i] = new List<int>();
        verts = new List<Vector3>();

        colors = new Color[0];
        poly = new LinkVert[0];

        bool init = true;

        this.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (!init)
            Start();

        UpdateMesh();

    }


    private void UpdateMesh()
    {
        verts.Clear();

        Array.ForEach(triangles, x=>x.Clear() );

        mf.sharedMesh.Clear();

        


        for (int pathIndex = 0; pathIndex<td.polyPathCount;pathIndex++)
        {
            int pathLength = td.polyPaths[pathIndex].size;
            Vector2[] path = td.polyPaths[pathIndex].verts.ToArray();

            int matIndex = td.polyPaths[pathIndex].textureID;

            int vertStart = verts.Count;

            Color col = Color.white;

            if(pathLength < 3)
            {
                Debug.LogError(gameObject + " has a degenerate path");
                this.enabled = false;
                return;
            }  else if(pathLength < 4)
            {
                

                for (int vertI = 0; vertI < path.Length; vertI++)
                     verts.Add(path[vertI]);


                triangles[matIndex].Add(vertStart);
                triangles[matIndex].Add(vertStart+1);
                triangles[matIndex].Add(vertStart+2);

                Array.Resize(ref colors, verts.Count);

                colors[vertStart] = col;
                colors[vertStart + 1] = col;
                colors[vertStart + 2] = col;

            } else
            {
                Array.Resize(ref poly, pathLength);
                LinkVert prev = null;
                LinkVert v = null;
                for (int vertI = 0; vertI < pathLength; vertI++)
                {

                    verts.Add(path[vertI]);

                    v = new LinkVert(path[vertI], vertStart+vertI, prev, null);

                    

                    if(prev != null)
                        prev.next = v;

                    prev = v;
                    
                    poly[vertI] = v;
                }
                poly[0].prev = v;
                v.next = poly[0];


                LinkVert currVert = poly[0];
                int startLen = poly.Length;
                int remaining = poly.Length;

                Array.Resize(ref colors, verts.Count);
                LinkVert toRem = null;
                int loop = remaining+4;
                //print("begin clipping");
                while (remaining > 3 && loop >= 0)
                {
                    loop--;
                    if (!isReflex(currVert) && isEar(currVert))
                    {

                        triangles[matIndex].Add(currVert.index);
                        triangles[matIndex].Add(currVert.next.index);
                        triangles[matIndex].Add(currVert.prev.index);

                        //col = Color.Lerp(Color.red,Color.blue,(float)(remaining-3)/(float)(startLen));


                        colors[currVert.index] = col;
                        colors[currVert.next.index] = col;
                        colors[currVert.prev.index] = col;

                        

                        toRem = currVert;
                        //print(toRem.index);

                        if (!isReflex(currVert.prev))
                            currVert = currVert.prev;
                        else currVert = currVert.next;

                        loop += 2;
                        remaining -= 1;
                    } else
                    {
                        colors[currVert.index] = col;
                        colors[currVert.next.index] = col;
                        colors[currVert.prev.index] = col;

                        currVert = currVert.next;
                    }
                    
                    if(toRem != null)
                    {

                        LinkVert.CollapseVert(toRem);
                        toRem = null;
                    }
                }
                if (loop < 0)
                {
                    Debug.LogError("No triangulation found");
                    this.enabled = false;
                }


                    for (int i = 0; i < 3; i++)
                    {
                        triangles[matIndex].Add(currVert.index);

                        //if (isReflex(currVert))
                        //    col = Color.black;
                        //else
                            //col = Color.green;

                    

                        colors[currVert.index] = col;

                        currVert = currVert.next;

                    }

            }

        }
        mf.sharedMesh.SetVertices(verts);
        mf.sharedMesh.subMeshCount = triangles.Length;
        for(int triI=0;triI < triangles.Length; triI++)
            mf.sharedMesh.SetTriangles(triangles[triI],triI);
        mf.sharedMesh.SetColors(new List<Color>(colors));
        

        Array.Clear(poly, 0, poly.Length);

    }

    private bool isReflex(LinkVert vert)
    {
        Vector2 p0 = vert.pos;
        Vector2 p1 = vert.prev.pos;
        Vector2 p2 = vert.next.pos;

        Vector2 v2 = p1 - p0;
        Vector2 v1 = p2 - p0;

        float ang = Mathf.Atan2(v2.y, v2.x) - Mathf.Atan2(v1.y, v1.x);

        if (ang > Mathf.PI * 2) ang -= Mathf.PI * 2;
        if (ang < 0) ang += Mathf.PI * 2;

        return ang <= Mathf.PI;


    }

    private bool isEar(LinkVert vert)
    {
        LinkVert curr = vert.next.next;
        while(curr != vert.prev)
        {

            // Compute vectors        
            Vector2 v0 = vert.prev.pos - vert.pos;
            Vector2 v1 = vert.next.pos - vert.pos;
            Vector2 v2 = curr.pos - vert.pos;

            // Compute dot products
            float dot00 = Vector2.Dot(v0, v0);
            float dot01 = Vector2.Dot(v0, v1);
            float dot02 = Vector2.Dot(v0, v2);
            float dot11 = Vector2.Dot(v1, v1);
            float dot12 = Vector2.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            if ((u >= 0- Vector2.kEpsilon) && (v >= 0- Vector2.kEpsilon) && (u + v <= 1+Vector2.kEpsilon)) return false;
            curr = curr.next;
        }

        return true;
    }

}
