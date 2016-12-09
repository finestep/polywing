using UnityEngine;

namespace PolyUtil
{




    class LinkVert
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


    }

}
