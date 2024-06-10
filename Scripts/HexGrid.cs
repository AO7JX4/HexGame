
using UnityEngine;


public class HexGrid : MonoBehaviour
{
    private int chunkCountX, chunkCountY;
    [SerializeField] HexMapEditor mEditor;
    [SerializeField] ColorData colorData;
    [SerializeField] HexChunk chunkPrefab;
    [SerializeField] HexCell cellPrefab;
    private HexChunk[] chunks;
    private HexCell[] cells;


    private int cellCountX = HexMetrics.chunkSizeX;
    private int cellCountY = HexMetrics.chunkSizeY;
    private int csize;

    public ColorData ColorData { get => colorData; set => colorData = value; }

    public int GetChunkCountX()
    { return chunkCountX; }

    public int GetChunkCountY() 
    {  return chunkCountY; }

    public int GetCellCountX()
    { return cellCountX; }
    public int GetCellCountY() 
    {  return cellCountY; }

    public int GetGridSizeX()
    { return chunkCountX*cellCountX; }

    public int GetGridSizeY() 
    {  return chunkCountY*cellCountY; }


    public HexCell[] GetCells()
    {
        return cells;
    }

    void Awake()
    {
        chunkCountX = StaticGameOptions.size;
        chunkCountY = StaticGameOptions.size;
        csize = cellCountX * cellCountY;
        chunks = new HexChunk[chunkCountX * chunkCountY];
        cells = new HexCell[cellCountX * chunkCountX * cellCountY * chunkCountY];

        for (int i = 0; i < chunkCountX* chunkCountY; i++)
        {
            CreateChunks(i);
        }

        CreateCells();
        for(int i= 0; i < cells.Length;  i++)
        {
            ConnectCells(cells[i]);
        }
        FillChunks();
        SetCellRow();
     
    }



    private void Start()
    {
        cells[0].gameObject.SetActive(true);
    }




    private void FillChunks()
    {
        for(int gridcol=0; gridcol<chunkCountX; gridcol++)
        {
            for (int gridrow = 0; gridrow < chunkCountY; gridrow++)
            {

                for (int chunkrow = 0; chunkrow < cellCountY; chunkrow++)
                {
                    for (int chunkcol = 0; chunkcol < cellCountX; chunkcol++)
                    {
                        cells[gridrow * cellCountY + chunkrow + cellCountY*chunkCountY * chunkcol+gridcol*chunkCountY*csize].transform.SetParent(chunks[chunkCountY*gridcol+gridrow].transform, false);
                    }
                }
            }
        }

    }

    private void SetCellRow()
    {
        for(int i=0; i< cells.Length;i++)
        {
            cells[i].SetRow(i%(chunkCountY*cellCountY));
        }
    }

  

    private void ConnectCells(HexCell cell)
    {
       for(int j=0; j < cells.Length; j++)
       {
            for (int i = 0; i < HexDirection.directionVectors.Length; i++)
            {
                if (cells[j].GetCoordinates().Q + HexDirection.directionVectors[i].Q==cell.GetCoordinates().Q && 
                    cells[j].GetCoordinates().R + HexDirection.directionVectors[i].R == cell.GetCoordinates().R && 
                    cells[j].GetCoordinates().S + HexDirection.directionVectors[i].S == cell.GetCoordinates().S)
                {
                    cell.AddNeighbour(cells[j]);
                }

            }
        }

    }

    private void CreateCells()
    {

        for (int q = 0, i = 0; q < cellCountX * chunkCountX; q++)
        {
            for (int r = 0; r < cellCountY * chunkCountY; r++)
            {
                CreateSingleCell(q, r, i++);
            }
        }

    }

    void CreateSingleCell(int q, int r, int i)
    {

        Vector3 position;
        position.x = (q + r * 0.5f - r / 2) * (HexMetrics.innerRadius * 2f);
        position.y = r * (HexMetrics.outerRadius * 1.5f);
        position.z = 0f;

        HexCell cell = Instantiate<HexCell>(cellPrefab); ;
        cell.transform.localPosition = position;
        cell.SetCoordinates(HexCoordinates.OffsetToCube(q, r));
        cells[i]=cell;



    }


    void CreateChunks(int i)
    {


        HexChunk chunk = Instantiate<HexChunk>(chunkPrefab);
        chunk.transform.SetParent(transform, false);
        chunk.transform.localPosition = transform.position;
        chunks[i] = chunk;

    }



}
