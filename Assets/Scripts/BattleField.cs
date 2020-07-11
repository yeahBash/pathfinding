using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class BattleField : MonoBehaviour
{
    const int BATTLEFIELD_SIZE = 50;
    private Grid _grid;
    [SerializeField] GameObject _cellNode;
    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    void Start()
    {
        CreateBattleField();
    }

    private void CreateBattleField()
    {
        for (int i = -BATTLEFIELD_SIZE/2; i < BATTLEFIELD_SIZE/2; i++)
        {
            for (int j = -BATTLEFIELD_SIZE/2; j < BATTLEFIELD_SIZE/2; j++)
            {
                GameObject cellNode = Instantiate(_cellNode, _grid.GetCellCenterLocal(new Vector3Int(i,j,0)), Quaternion.identity);
                cellNode.transform.SetParent(transform);
            }
        }
    }

}
