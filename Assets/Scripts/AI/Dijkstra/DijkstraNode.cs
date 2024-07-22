using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraNode 
{
    public Vector2Int gridPos;
    public DijkstraNode parentNode;
    public List<DijkstraNode> neighbours= new List<DijkstraNode>();
    public bool isObstacle = false;
    public int pickedOrder = 0;
    public int cost=0;
    public DijkstraNode(Vector2Int _gridPos)
    {
        gridPos = _gridPos;
    }
}
