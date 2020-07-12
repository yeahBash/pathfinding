using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    private BattleField _battleField;
    public static Action<NodeType, Node> OnBattleFieldChanged;
    [SerializeField] private Node _startNode;
    [SerializeField] private Node _finishNode;

    void Start()
    {
        _battleField = GetComponent<BattleField>();
        OnBattleFieldChanged += SetBattleFieldPoint;
    }

    public void SetBattleFieldPoint(NodeType nodeType, Node node)
    {
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
            /*case NodeType.Obstacle:
                if (node.NodeType == NodeType.Start) _startNode = null; 
                if (node.NodeType == NodeType.Finish) _finishNode = null;
                break;
            case NodeType.Walkable:
                if (node.NodeType == NodeType.Start) _startNode = null; 
                if (node.NodeType == NodeType.Finish) _finishNode = null;
                break;*/
        }

        Debug.Log(_startNode?.gameObject.name + " " + _finishNode?.gameObject.name);
        FindPath();
    }

    private void FindPath()
    {
        if (_startNode == null || _finishNode == null)
        {
            return;
        }

        Debug.Log("start searching");

        Manager.CurrentState = State.SearchingPath;
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
                Debug.Log("found");
                Manager.CurrentState = State.Idle;
                RetracePath(_startNode, _finishNode);
                return;
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
        
        Manager.CurrentState = State.Idle;
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
