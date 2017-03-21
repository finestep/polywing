using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using PolyUtil;

[RequireComponent(typeof(TerrainData))]
public class DeformTerrain : MonoBehaviour
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


    TerrainData td;

    // Use this for initialization
    void Start()
    {
        td = GetComponent<TerrainData>();
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

        LinkVert[] newVerts = new LinkVert[0];
        int currComponent = 0;


        LinkVert[] interHead = new LinkVert[td.polyPathCount];

        List<TerrainData.PolyPath> newPaths = new List<TerrainData.PolyPath>();

        var rad = c.rad;

        for (int pathI = 0; pathI < td.polyPathCount; pathI++)
        {
            Vector2[] path = td.polyPaths[pathI].verts.ToArray();

            intersections.Clear();
            unvisited.Clear();


            LinkVert prev = null;
            LinkVert vert = null;

            int contains = 0;

            vert = td.polyPaths[pathI].head;



            c.rad = rad * td.polyPaths[pathI].hardness;

            do
            {

                Vector2 pos = vert.pos;

                if (c.contains(pos))
                {
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
                            int1 = new LinkVert(pos1, path.Length + intersections.Count, vert, vert.next);
                            intersections.Add(int1);
                        }


                        if (0 < alpha2 && alpha2 < 1)
                        {
                            int2 = new LinkVert(pos2, path.Length + intersections.Count, int1 != null ? int1 : vert, vert.next);
                            if(int1 != null)
                                int1.next = int2;

                            intersections.Add(int2);
                        }
                    }
                }
                vert = vert.next;
            } while (vert != td.polyPaths[pathI].head);

            Debug.Assert(LinkVert.validateVerts(td.polyPaths[pathI].head));

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

                    Debug.Assert( LinkVert.validateVerts(interHead[pathI]) );
                }

                Debug.Assert(LinkVert.validateVerts(td.polyPaths[pathI].head));


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

               

                currComponent = 0;
                Array.Resize(ref newVerts, 1);


                LinkVert prevVert = null;

                LinkVert newVert = null;
                LinkVert prevNewVert = null;

                

                while (unvisited.Count > 0)
                {

                    if (currComponent >= newVerts.Length)
                        Array.Resize(ref newVerts, newVerts.Length + 1);

                    
                    LinkVert intersect = unvisited.First();
                    newVerts[currComponent] = intersect;

                    //Debug.Log(intersect.index + " : " + intersect.pos);

                    unvisited.Remove(intersect);

                    prevVert = intersect;
                    

                    LinkVert v = intersect.next;

                    do
                    {
                        

                        if (intersections.Contains(v) && !entry.Contains(v) )
                        {

                            LinkVert firstInt = v;

                            prevNewVert = v;

                            v = v.counterpart.prev.counterpart;
                            unvisited.Remove(v);

                            float dist = (firstInt.pos - v.pos).sqrMagnitude;

                            int subdivCount = Mathf.RoundToInt(dist * 0.8f );

                            float startAng = Mathf.PI + Mathf.Atan2(firstInt.pos.y - c.pos.y, firstInt.pos.x - c.pos.x);
                            float endAng = Mathf.PI + Mathf.Atan2(v.pos.y - c.pos.y, v.pos.x - c.pos.x);


                            if (endAng < startAng) startAng -= Mathf.PI * 2;

                            float step = (endAng - startAng) / (subdivCount+1);

                            Vector2 rotV = Vector2.zero;
                            for (float phi = startAng+step; phi <endAng; phi += step )
							{
                                rotV.x = Mathf.Cos(phi);
                                rotV.y = Mathf.Sin(phi);

                                newVert = new LinkVert(c.pos - rotV * c.rad, 0, prevNewVert);
                                prevNewVert.next = newVert;

                                prevNewVert = newVert;

                            }

                            
                            prevNewVert.next = v;
                            v.prev = prevNewVert;

                            //Debug.Assert(LinkVert.validateVerts(v));
                        } else v = v.next;

                        //Debug.Assert(LinkVert.validateVerts(v));

                    } while (v != intersect);


                    currComponent++;
                }

                foreach (LinkVert v in newVerts)
                {
                    Debug.Assert(LinkVert.validateVerts(v));
                    if (LinkVert.Circumference(v) < 1.0f) continue;
                    TerrainData.PolyPath pp = td.polyPaths[pathI];
                    pp.head = v;
                    pp.cleanCloseVerts();
                    pp.recalc();
                    newPaths.Add(pp);
                }

            } else if (contains != td.polyPaths[pathI].size) {
                newPaths.Add(td.polyPaths[pathI]);
            }




        }

        c.rad = rad;

        td.polyPaths = newPaths.ToArray();
        td.polyPathCount = newPaths.Count;

        td.UpdateCollider();
    }



}