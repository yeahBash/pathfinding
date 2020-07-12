using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    private static Node _startPoint;
    private static Node _finishPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Path(NodeType nodeType, Node node)
    {
        switch(nodeType)
        {
            case NodeType.Start:
                if (_startPoint != null && _startPoint != node)
                    _startPoint.NodeType = NodeType.NonObstacle;

                _startPoint = node;
                break;
            case NodeType.Finish:
                if (_finishPoint != null && _finishPoint != node)
                    _finishPoint.NodeType = NodeType.NonObstacle;

                _finishPoint = node;
                break;
        }
        
    }
}
