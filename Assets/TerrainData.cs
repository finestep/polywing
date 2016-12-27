using UnityEngine;
using System.Collections.Generic;
using System;
using PolyUtil;

public class TerrainData : MonoBehaviour {

	public struct PolyPath {
		public List<Vector2> verts;
		public LinkVert head;
		public int size { get; private set; }
		int textureID { get; private set; }
		float hardness { get; private set; }
		public PolyPath() {
		}
		public PolyPath(Vector2[] path) {
			
			LinkVert v,prev = null;
			for( int i = 0; i < verts.Count; i++) {
				verts.Add(path[i]);

				v = new LinkVert(path[i], i, prev, null);

				if(prev != null)
					prev.next = v;
				else
					head = v;

				prev = v;
			}
			head.prev = v;
			v.next = head;

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

			polyPaths = new PolyPath[pc.pathCount];

			for (int pathI = 0; pathI < pc.pathCount; pathI++) {
				PolyPath pp = new PolyPath (pc.GetPath (pathI));

			}
		} else {
			polyPaths = null;
			polyPathCount = 0;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
	}
}
