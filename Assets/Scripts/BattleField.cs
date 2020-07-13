using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class BattleField : MonoBehaviour
{
    private Texture2D originalTexture; 
    [SerializeField] private Texture2D pathTexture; 
    const int BATTLEFIELD_SIZE = 50;
    private Grid _grid;

    private List<Node> _path;
    private Node[,] _nodes = new Node[BATTLEFIELD_SIZE, BATTLEFIELD_SIZE];
    [SerializeField] GameObject _cells;
    [SerializeField] GameObject _cellNode;
    
    private void Awake()
    {
        _grid = GetComponent<Grid>();
        if (_grid.cellSwizzle != GridLayout.CellSwizzle.XYZ && _grid.cellSwizzle != GridLayout.CellSwizzle.YXZ) 
        {
            _grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
            Debug.LogAssertion("Grid cell swizzie must be XYZ or YXZ. Changed to XYZ.");
        }

        originalTexture = _cellNode.GetComponent<MeshRenderer>().sharedMaterial.mainTexture as Texture2D;
    }

    void Start()
    {
        CreateBattleField();

        // change camera size according to cell size and gap
        Camera.main.orthographicSize *= Mathf.Max(_grid.cellSize.x, _grid.cellSize.y) + Mathf.Max(_grid.cellGap.x, _grid.cellGap.y);
    }

    public void DrawPath(List<Node> path)
    {
        ResetPath();
        _path = path;
        SetPathTexture(pathTexture);
    }

    public void ResetPath()
    {
        if (_path != null)
        {
            SetPathTexture(originalTexture);
            _path = null;
        }
    }
    private void SetPathTexture(Texture2D texture)
    {
        foreach (Node p in _path)
        {
            p.Texture = texture;
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                if (checkX >= 0 && checkX < BATTLEFIELD_SIZE && checkY >= 0 && checkY < BATTLEFIELD_SIZE)
                {
                    neighbours.Add(_nodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    private void CreateBattleField()
    {
        for (int i = -BATTLEFIELD_SIZE/2; i < BATTLEFIELD_SIZE/2; i++)
        {
            for (int j = -BATTLEFIELD_SIZE/2; j < BATTLEFIELD_SIZE/2; j++)
            {
                GameObject cellNode = Instantiate(_cellNode, _grid.GetCellCenterLocal(new Vector3Int(i,j,0)), Quaternion.identity);
                cellNode.transform.SetParent(_cells.transform);
                cellNode.transform.localScale = _grid.cellLayout == GridLayout.CellLayout.Rectangle ? _grid.cellSize : 0.5f*_grid.cellSize;
                
                // TODO depend on cell swizzie 

                int x = i + BATTLEFIELD_SIZE/2;
                int y = j + BATTLEFIELD_SIZE/2;
                cellNode.name = x.ToString() + " " + y.ToString();

                Node node;
                node = cellNode.GetComponent<Node>();
                node.x = x;
                node.y = y;
                _nodes[x,y] = node;
            }
        }
    }

}
