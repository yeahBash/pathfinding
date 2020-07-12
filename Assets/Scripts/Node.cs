using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    // TODO make reference from EditMode
    Start,
    Finish,
    Obstacle,
    Walkable
}
[RequireComponent(typeof(MeshRenderer))]
public class Node : MonoBehaviour
{
    [SerializeField] private NodeType _nodeType;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    } 

    public Node parent;
    
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
            CellColor = Manager.EditModeToVisual[(EditMode)value].color;
            _previousColor = CellColor;
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
    public Texture2D Texture 
    {
        set
        {
            _material.mainTexture = value;
        }
    }


    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;

        // TODO
        _nodeType = NodeType.Walkable;
        CellColor = Manager.EditModeToVisual[(EditMode)_nodeType].color;
        _previousColor = CellColor;
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
        NodeType managerMode = (NodeType)Manager.CurrentEditMode;
        if (Manager.CurrentState == State.Idle && NodeType != managerMode)
        {
            NodeType previousNodeType = NodeType;
            NodeType = managerMode;
            Astar.OnBattleFieldChanged(previousNodeType, NodeType, this);
        }
    }
}
