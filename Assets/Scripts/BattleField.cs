using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class BattleField : MonoBehaviour
{
    const int BATTLEFIELD_SIZE = 50;
    private Grid _grid;
    private Node[,] _nodes = new Node[BATTLEFIELD_SIZE, BATTLEFIELD_SIZE];
    [SerializeField] GameObject _cells;
    [SerializeField] GameObject _cellNode;
    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    void Start()
    {
        CreateBattleField();
        Camera.main.orthographicSize *= Mathf.Max(_grid.cellSize.x, _grid.cellSize.y) + Mathf.Max(_grid.cellGap.x, _grid.cellGap.y);
    }

    private void CreateBattleField()
    {
        for (int i = -BATTLEFIELD_SIZE/2; i < BATTLEFIELD_SIZE/2; i++)
        {
            for (int j = -BATTLEFIELD_SIZE/2; j < BATTLEFIELD_SIZE/2; j++)
            {
                GameObject cellNode = Instantiate(_cellNode, _grid.GetCellCenterLocal(new Vector3Int(i,j,0)), Quaternion.identity);
                cellNode.transform.SetParent(_cells.transform);
                cellNode.transform.localScale = _grid.cellSize;

                int x = i + BATTLEFIELD_SIZE/2;
                int y = j + BATTLEFIELD_SIZE/2;
                cellNode.name = x.ToString() + " " + y.ToString();

                _nodes[x,y] = cellNode.GetComponent<Node>();
            }
        }
    }

}
