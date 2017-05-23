using UnityEngine;

namespace PolyUtil
{

    
public class LinkVert
    {
        public LinkVert prev;
        public LinkVert next;

        public LinkVert counterpart;

        public int index;

        public Vector2 pos;

        

        public LinkVert(Vector2 position, int indexI, LinkVert prevV = null, LinkVert nextV = null, LinkVert other = null)
        {
            pos = position;
            index = indexI;
            prev = prevV;
            next = nextV;
            counterpart = other;
        }
        public LinkVert(LinkVert other)
        {
            pos = other.pos;
            index = other.index;
            prev = other.prev;
            next = other.next;
            counterpart = other.counterpart;
        }
        public static void InsertVert(LinkVert a, LinkVert b, LinkVert middle)
        {
            a.next = middle;
            b.prev = middle;
            middle.prev = a;
            middle.next = b;
        }

        public static void CollapseVert(LinkVert a)
        {
            a.prev.next = a.next;
            a.next.prev = a.prev;
        }



        public static void SplitEdge(LinkVert a, LinkVert b, Vector2? pos = null)
        {
            if (pos == null) pos = (a.pos + b.pos) * 0.5f;
            LinkVert v = new LinkVert((Vector2)pos, -1, a, b);
            InsertVert(a, b, v);
        }

        public static int Length(LinkVert start)
        {
            LinkVert v = start;
            LinkVert prev = v.prev;

            int x = 0;

            do
            {
                x += 1;

                prev = v;

                v = v.next;
            } while (v != start);

            return x;
        }

        public static float Circumference(LinkVert start)
        {
            LinkVert v = start;
            LinkVert prev = v.prev;

            float x = 0;

            do
            {
                if (v == null || prev==null)
                    Debug.Assert(false);
                x += (v.pos - prev.pos).magnitude;
               
                prev = v;

                v = v.next;
            } while (v != start);

            return x;
        }

        public static void printVerts(LinkVert vert)
        {
            int i = 0;
            LinkVert v = vert;
            do
            {
                Debug.Log("Vert " + v.index + " , pos:" + v.pos);
                v = v.next;
                i++;
            } while (v != vert && i < 20);
        }

        public static bool validateVerts(LinkVert start)
        {
            int i = 0;
            LinkVert v = start;
            do
            {
                if (v == null || v.prev == null || v.next==null
                    || v.prev.next != v || v.next.prev != v)
                    return false;
                v = v.next;
                i++;
            } while (v != start && i < 100);

            if(i==100)
                return false;

            return true;
        }

    }

}
