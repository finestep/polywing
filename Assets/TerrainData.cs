using UnityEngine;
using System.Collections.Generic;
using System;
using PolyUtil;

public class TerrainData : MonoBehaviour {

	public struct PolyPath {
		public List<Vector2> verts;
		public LinkVert head;
		public int size { get; private set; }
		public int textureID { get; private set; }
        public float hardness { get; private set; } // direct multiplier on explosion radius


		public PolyPath(Vector2[] path,int texID = 0, float hard = 1) {

            verts = new List<Vector2>();
            head = null;

            textureID = texID;
            hardness = hard;

            if (path.Length >= 3)
            {
                LinkVert v = null, prev = null;
                
                for (int i = 0; i < path.Length; i++)
                {
                    verts.Add(path[i]);

                    v = new LinkVert(path[i], i, prev, null);

                    if (prev != null)
                        prev.next = v;
                    else
                        head = v;

                    prev = v;
                }
                head.prev = v;
                v.next = head;
                size = path.Length;
            } else
            {
                size = 0;
            }

		}

		public void addVert(LinkVert v) {
			LinkVert.InsertVert(head,head.prev,v);
			verts.Add(v.pos);
			size++;
		}

		public void recalc() {
			LinkVert v = head;
			int n = 0;
			do {
				verts.Add( v.pos );
				v = v.next;
				n++;
			} while( v != head);
			verts.RemoveRange (n, verts.Count - n);
			size = n;
		}
	}

	public PolyPath[] polyPaths;
	public int polyPathCount;

	public int[] initialMats;
	public int[] initialHard;

	public bool fromColliderOnStart;

	// Use this for initialization
	void Start () {
		if (fromColliderOnStart) {

			PolygonCollider2D pc = GetComponent<PolygonCollider2D> ();

            if (!pc)
            {
                Debug.LogError("No collider to initialize TerrainData from");
                return;
            }

			polyPaths = new PolyPath[pc.pathCount];

            bool initialArraysValid = initialHard != null && initialMats != null &&
                initialMats.Length == pc.pathCount && initialHard.Length == pc.pathCount;

            if (!initialArraysValid) Debug.LogWarning("initial TerrainData array size mismatch");

            PolyPath pp;

            for (int pathI = 0; pathI < pc.pathCount; pathI++) {
                if (initialArraysValid)
                    pp = new PolyPath(pc.GetPath(pathI), initialMats[pathI], initialHard[pathI]);
                else
                    pp = new PolyPath(pc.GetPath(pathI));
                polyPaths[pathI] = pp;
			}
            polyPathCount = pc.pathCount;
		} else {
			polyPaths = null;
			polyPathCount = 0;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
	}
}
