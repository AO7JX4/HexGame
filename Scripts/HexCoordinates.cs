using UnityEngine;
[System.Serializable]
public struct HexCoordinates
{
    private int q, r, s;

    public HexCoordinates(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.s = -q - r;
    }

    public static HexCoordinates OffsetToCube(int q, int r)
    {
        int newq = q - (r - (r & 1)) / 2;
        int newr = r;
        int news = -newq - newr;

        return new HexCoordinates(newq, newr);
    }

    public int Q { get { return q; } }
    public int R { get { return r; } }
    public int S { get { return s; } }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float q = (Mathf.Sqrt(3f) / 3f * position.x - 1f / 3f * position.y) / HexMetrics.outerRadius;
        float r = (2f / 3f * position.y) / HexMetrics.outerRadius;
        return HexRound(q, r);
    }

    private static HexCoordinates HexRound(float q, float r)
    {
        float s = -q - r;
        int roundedQ = Mathf.RoundToInt(q);
        int roundedR = Mathf.RoundToInt(r);
        int roundedS = Mathf.RoundToInt(s);

        float qDiff = Mathf.Abs(roundedQ - q);
        float rDiff = Mathf.Abs(roundedR - r);
        float sDiff = Mathf.Abs(roundedS - s);

        if (qDiff > rDiff && qDiff > sDiff)
        {
            roundedQ = -roundedR - roundedS;
        }
        else if (rDiff > sDiff)
        {
            roundedR = -roundedQ - roundedS;
        }

        return new HexCoordinates(roundedQ, roundedR);
    }

    private static HexCoordinates CubeSubtract(HexCell cell1, HexCell cell2)
    {
        return new HexCoordinates(cell1.GetCoordinates().Q-cell2.GetCoordinates().Q, cell1.GetCoordinates().R - cell2.GetCoordinates().R);
    }


    public static int GetDistance(HexCell cell1, HexCell cell2)
    {
        HexCoordinates vec=CubeSubtract(cell1, cell2);
        return (Mathf.Abs(vec.q) + Mathf.Abs(vec.r) + Mathf.Abs(vec.s)) / 2;
    }


}
