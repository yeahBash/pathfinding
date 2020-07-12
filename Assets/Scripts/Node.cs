using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    // TODO make reference from EditMode
    Start,
    Finish,
    Obstacle,
    NonObstacle
}
public class Node : MonoBehaviour
{
    [SerializeField] private NodeType _nodeType;
    public NodeType NodeType
    {
        get
        {
            return _nodeType;
        }
        set
        {
            // TODO change color
            _nodeType = value;
            CellColor = Manager.ModeToVisual[(EditMode)value].color;
            _previousColor = CellColor;
            Astar.Path(value, this);
        }
    }

    private Material _material;
    [SerializeField] private Color _cellColor;

    private Color CellColor
    {
        get 
        {
            return _cellColor;
        }
        set
        {
            _cellColor = value;
            _material.color = value;
        }
    }
    [SerializeField] private Color _previousColor;
    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        NodeType = NodeType.NonObstacle;
    }

    void OnMouseEnter()
    {
        _previousColor = CellColor;
        CellColor = Manager.CurrentColor;
    }

    void OnMouseExit()
    {
        CellColor = _previousColor;
    }

    void OnMouseDown()
    {
        NodeType = (NodeType)Manager.CurrentMode;
    }
}
