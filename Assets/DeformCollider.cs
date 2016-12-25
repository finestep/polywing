using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using PolyUtil;

[RequireComponent(typeof(PolygonCollider2D))]
public class DeformCollider : MonoBehaviour
{

    public struct Circle
    {
        public Vector2 pos;
        public float rad;
        public Circle(Vector2 position, float radius)
        {
            pos = position;
            rad = radius;
        }
        public bool contains(Vector2 other)
        {
            if (other.x < pos.x - rad || other.x > pos.x + rad) return false;
            if (other.y < pos.y - rad || other.y > pos.y + rad) return false;

            return Vector2.Distance(pos, other) < rad;
        }
    }


    PolygonCollider2D pc;

    // Use this for initialization
    void Start()
    {
        pc = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Deform(Circle c)
    {
        
        c.pos -= (Vector2)transform.position;

        List<LinkVert> intersections = new List<LinkVert>();
        HashSet<LinkVert> unvisited = new HashSet<LinkVert>();

        List<Vector2>[] newVerts = new List<Vector2>[0];
        int currComponent = 0;

        LinkVert[] polyHead = new LinkVert[pc.pathCount];
        LinkVert[] interHead = new LinkVert[pc.pathCount];

        bool[] modified = new bool[pc.pathCount];
        bool[] toRemove = new bool[pc.pathCount];

        for (int pathI = 0; pathI < pc.pathCount; pathI++)
        {
            Vector2[] path = pc.GetPath(pathI);

            intersections.Clear();
            unvisited.Clear();


            LinkVert prev = null;
            LinkVert vert = null;

            int contains = 0;


            for (int vertI = 0; vertI < path.Length; vertI++)
            {

                vert = new LinkVert(path[vertI], vertI, prev, null);

                if (prev != null)
                    prev.next = vert;
                else
                    polyHead[pathI] = vert;

                prev = vert;

            }
            vert.next = polyHead[pathI];
            polyHead[pathI].prev = vert;

            vert = polyHead[pathI];

            do
            {
                Vector2 pos = vert.pos;

                if (c.contains(pos))
                {
                    modified[pathI] = true;
                    contains++;
                }

                Vector2 delta = vert.next.pos - pos;


                //Solve quadratic to find intersections
                float quadA = delta.sqrMagnitude;
                float quadB = 2 * (Vector2.Dot(pos, delta) - Vector2.Dot(c.pos, delta));
                float quadC = pos.sqrMagnitude - 2 * Vector2.Dot(pos, c.pos)
                                + c.pos.sqrMagnitude - c.rad * c.rad;
                float discr = quadB * quadB - 4 * quadA * quadC;

                if (discr > 0)
                {



                    float alpha1 = (-quadB - Mathf.Sqrt(discr)) / (2 * quadA);
                    float alpha2 = (-quadB + Mathf.Sqrt(discr)) / (2 * quadA);



                    Vector2 pos1 = pos + delta * alpha1;
                    Vector2 pos2 = pos + delta * alpha2;

                    LinkVert int1 = null, int2 = null;

                    if (Mathf.Abs(alpha1 - alpha2) > 0.075f)
                    {
                        if (0 < alpha1 && alpha1 < 1)
                        {
                            modified[pathI] = true;
                            int1 = new LinkVert(pos1, path.Length + intersections.Count, vert, vert.next);
                            intersections.Add(int1);
                        }


                        if (0 < alpha2 && alpha2 < 1)
                        {
                            modified[pathI] = true;
                            int2 = new LinkVert(pos2, path.Length + intersections.Count, int1 != null ? int1 : vert, vert.next);
                            if(int1 != null)
                                int1.next = int2;

                            intersections.Add(int2);
                        }
                    }
                }
                vert = vert.next;
            } while (vert != polyHead[pathI]);

            if (contains == path.Length)
                toRemove[pathI] = true;

            if (intersections.Count > 0)
            {

                intersections.Sort((a, b) => {
                    float ang1 = Mathf.PI+Mathf.Atan2(a.pos.y - c.pos.y, a.pos.x - c.pos.x);
                    float ang2 = Mathf.PI+Mathf.Atan2(b.pos.y - c.pos.y, b.pos.x - c.pos.x);

                    float diff = ang2 - ang1;

                    if (diff < 0) return -1;
                    else if (diff > 0) return 1;
                    else return 0;
                });

                LinkVert circleInt = null, prevInt = null;
                foreach (LinkVert inter in intersections)
                {

                    //Debug.Log(Mathf.Atan2(inter.pos.y - c.pos.y, inter.pos.x - c.pos.x));

                    circleInt = new LinkVert(inter);

                    circleInt.counterpart = inter;
                    inter.counterpart = circleInt;

                    if (prevInt == null) interHead[pathI] = circleInt;
                    else
                    {
                        prevInt.next = circleInt;
                        circleInt.prev = prevInt;
                    }

                    prevInt = circleInt;

                    LinkVert.InsertVert(inter.prev, inter.next, inter);
                }



                if (interHead[pathI] != null)
                {
                    circleInt.next = interHead[pathI];
                    interHead[pathI].prev = circleInt;
                }







                HashSet<LinkVert> entry = new HashSet<LinkVert>();
                //int n = 1;
                foreach (LinkVert inter in intersections)
                {
                    if (intersections.Contains(inter.prev) || c.contains(inter.prev.pos))
                        entry.Add(inter);
                    //for(int i=0;i<n;i++)
                    //    Debug.DrawRay((Vector2)transform.position+inter.pos+Vector2.up*i*0.1f, transform.right*0.2f,Color.red,10);
                    //n++;
                }
                unvisited.UnionWith(entry);

                int subdivCount = Mathf.RoundToInt(Math.Max(1, c.rad * 3.1f) );

                while (unvisited.Count > 0)
                {

                    if (currComponent >= newVerts.Length)
                        Array.Resize(ref newVerts, newVerts.Length + 1);

                    newVerts[currComponent] = new List<Vector2>();
                    LinkVert intersect = unvisited.First();

                    unvisited.Remove(intersect);



                    newVerts[currComponent].Add(intersect.pos);

                    LinkVert v = intersect.next;

                    do
                    {

                        newVerts[currComponent].Add(v.pos);

                        //Debug.Log(v.index);

                        if (intersections.Contains(v) && !entry.Contains(v) )
                        {

                            LinkVert firstInt = v;

                            v = v.counterpart.prev.counterpart;
                            unvisited.Remove(v);

                            float startAng = Mathf.PI + Mathf.Atan2(firstInt.pos.y - c.pos.y, firstInt.pos.x - c.pos.x);
                            float endAng = Mathf.PI + Mathf.Atan2(v.pos.y - c.pos.y, v.pos.x - c.pos.x);


                            if (endAng < startAng) startAng -= Mathf.PI * 2;

                            float step = (endAng - startAng) / (subdivCount+1);

                            Vector2 rotV = Vector2.zero;
                            for (float phi = startAng+step; phi <endAng; phi += step )
							{
                                rotV.x = Mathf.Cos(phi);
                                rotV.y = Mathf.Sin(phi);
                                newVerts[currComponent].Add(c.pos - rotV * c.rad);
                            }

                        }
                        else v = v.next;

                    } while (v != intersect);






                    currComponent++;
                }

            }

        }

  



        for(int pathI = 0;pathI<pc.pathCount;pathI++)
            if (toRemove[pathI])
            {
                for (int replaceI = pathI; replaceI < pc.pathCount - 1; replaceI++)
                    pc.SetPath(replaceI, pc.GetPath(replaceI + 1));
                pc.pathCount--;
            }

        int pathPos = 0;
        int newPathN = 0;

        while(newPathN < newVerts.Length )
        {
            if (pathPos >= pc.pathCount)
                pc.pathCount++;

            if (pathPos >= modified.Length || modified[pathPos] )
            {
                pc.SetPath(pathPos, newVerts[newPathN].ToArray());
                newPathN++;

            }
            pathPos++;
            
        }

    }

}