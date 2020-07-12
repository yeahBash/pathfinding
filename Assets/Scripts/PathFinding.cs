using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleField))]
public class PathFinding : MonoBehaviour
{
    private BattleField _battleField;
    [SerializeField] private UnityEngine.UI.Text text;
    public static Action<NodeType, NodeType, Node> OnBattleFieldChanged;
    [SerializeField] private Node _startNode;
    [SerializeField] private Node _finishNode;

    void Start()
    {
        _startNode = _finishNode = null;
        _battleField = GetComponent<BattleField>();
        OnBattleFieldChanged += SetBattleFieldNode;
    }

    public void SetBattleFieldNode(NodeType previousNodeType, NodeType nodeType, Node node)
    {
        if (previousNodeType == NodeType.Start) _startNode = null; 
        if (previousNodeType == NodeType.Finish) _finishNode = null;

        switch(nodeType)
        {
            case NodeType.Start:
                if (_startNode != null && _startNode != node)
                    _startNode.NodeType = NodeType.Walkable;

                _startNode = node;
                break;
            case NodeType.Finish:
                if (_finishNode != null && _finishNode != node)
                    _finishNode.NodeType = NodeType.Walkable;

                _finishNode = node;
                break;
        }

        if (_startNode == null || _finishNode == null)
        {
            _battleField.ResetPath();
            return;
        }

        Manager.CurrentState = State.SearchingPath;
        text.text = "Searching path...";
        OnSearchingComplete(AstarPathFinding());
    }

    private void OnSearchingComplete(bool isFound)
    {
        text.text = isFound ? "found" : "not found";
        Manager.CurrentState = State.Idle;

        if (isFound)
        {
            RetracePath(_startNode, _finishNode);
        } 
        else
        {
            _battleField.ResetPath();
        }
    }

    private bool AstarPathFinding()
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(_startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == _finishNode)
            {
                return true;
            }
                
            foreach(Node neighbour in _battleField.GetNeighbours(currentNode))
            {
                if (neighbour.NodeType == NodeType.Obstacle || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, _finishNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }

        }
        
        return false;
    }

    private void RetracePath(Node startNode, Node finishNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = finishNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        _battleField.DrawPath(path);
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);

        if (dstX > dstY)
            return 14*dstY + 10*(dstX-dstY);
        
        return 14*dstX + 10*(dstY-dstX);
    }
}
