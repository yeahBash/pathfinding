using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool IsObstacle {get; set;}
    private Material _material;
    private Color _previousColor;
    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    void OnMouseEnter()
    {
        _previousColor = _material.color;
        _material.color = Manager.CurrentColor;
    }

    void OnMouseExit()
    {
        _material.color = _previousColor;
    }

    void OnMouseDown()
    {
        _previousColor = Manager.CurrentColor;
    }

}
