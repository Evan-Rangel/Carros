using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DijkstraAI : MonoBehaviour
{
    int gridSizeX = 50;
    int gridSizeY = 30;
    float cellSize = 2;
    DijkstraNode[,] dijkstraNodes = null;
    DijkstraNode startNode;

    List<DijkstraNode> OpenList = new List<DijkstraNode>();
    List<DijkstraNode> ClosedList = new List<DijkstraNode>();

    List<Vector2> aiPath= new List<Vector2>();

    [SerializeField] bool isDebugActiveForCar = true;

    //Debug 
    Vector3 startPositionDebug = new Vector3(1000, 0, 0);
    Vector3 destinationPositionDebug = new Vector3(1000, 0, 0);

    private void Start()
    {
        CreateGrid();
    }
    void CreateGrid()
    {
        dijkstraNodes = new DijkstraNode[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                dijkstraNodes[x, y] = new DijkstraNode(new Vector2Int(x, y));
                Vector3 worldPosition = ConvertGridPositionToWorldPosition(dijkstraNodes[x, y]);
                Collider2D hitColl2D = Physics2D.OverlapCircle(worldPosition, cellSize / 2f);
                if (hitColl2D != null)
                {
                    if (hitColl2D.transform.root.CompareTag("AI") || hitColl2D.transform.root.CompareTag("Player") || hitColl2D.transform.CompareTag("CheckPoint") || hitColl2D.transform.CompareTag("Waypoint"))
                        continue;
                    dijkstraNodes[x, y].isObstacle = true;
                }
                dijkstraNodes[x, y].cost = int.MaxValue;
            }
        }
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (y - 1 >= 0)
                {
                    if (!dijkstraNodes[x, y - 1].isObstacle)
                        dijkstraNodes[x, y].neighbours.Add(dijkstraNodes[x, y - 1]);
                }

                //Check neighbour to south, if we are on the edge then don't add it
                if (y + 1 <= gridSizeY - 1)
                {
                    if (!dijkstraNodes[x, y + 1].isObstacle)
                        dijkstraNodes[x, y].neighbours.Add(dijkstraNodes[x, y + 1]);
                }

                //Check neighbour to east, if we are on the edge then don't add it
                if (x - 1 >= 0)
                {
                    if (!dijkstraNodes[x - 1, y].isObstacle)
                        dijkstraNodes[x, y].neighbours.Add(dijkstraNodes[x - 1, y]);
                }

                //Check neighbour to west, if we are on the edge then don't add it
                if (x + 1 <= gridSizeX - 1)
                {
                    if (!dijkstraNodes[x + 1, y].isObstacle)
                        dijkstraNodes[x, y].neighbours.Add(dijkstraNodes[x + 1, y]);
                }
            }
        }
    }
    Vector2Int ConvertWorldToGridPoint(Vector2 position)
    {
        //Calculate grid point
        Vector2Int gridPoint = new Vector2Int(Mathf.RoundToInt(position.x / cellSize + gridSizeX / 2.0f), Mathf.RoundToInt(position.y / cellSize + gridSizeY / 2.0f));

        return gridPoint;
    }
    Vector2Int ConvertWorldToGridPoint(Vector2Int position)
    {
        //Calculate grid point
        Vector2Int gridPoint = new Vector2Int(Mathf.RoundToInt(position.x / cellSize + gridSizeX / 2.0f), Mathf.RoundToInt(position.y / cellSize + gridSizeY / 2.0f));

        return gridPoint;
    }
    Vector3 ConvertGridPositionToWorldPosition(DijkstraNode dijktraNode)
    {
        return new Vector3(dijktraNode.gridPos.x * cellSize - (gridSizeX * cellSize) / 2.0f, dijktraNode.gridPos.y * cellSize - (gridSizeY * cellSize) / 2.0f, 0);
    }
    Vector3 ConvertGridPositionToWorldPosition(Vector2 gridPos)
    {
        return new Vector3(gridPos.x * cellSize - (gridSizeX * cellSize) / 2.0f, gridPos.y * cellSize - (gridSizeY * cellSize) / 2.0f, 0);
    }
    DijkstraNode GetNodeFromPoint(Vector2Int gridPoint)
    {
        if (gridPoint.x < 0)
            return null;

        if (gridPoint.x > gridSizeX - 1)
            return null;

        if (gridPoint.y < 0)
            return null;

        if (gridPoint.y > gridSizeY - 1)
            return null;

        return dijkstraNodes[gridPoint.x, gridPoint.y];
    }
    private void Reset()
    {
        OpenList.Clear();
        ClosedList.Clear();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                dijkstraNodes[x, y].cost = int.MaxValue;
            }
        }
    }


    public List<Vector2> StartPathfinding(Vector2 target)
    {
        if (dijkstraNodes == null) return null;
        Reset();

        Vector2Int destinationGridPoint = ConvertWorldToGridPoint(target);
        Vector2Int currentGridPoint = ConvertWorldToGridPoint(transform.position);
        startPositionDebug = target;
        destinationPositionDebug = transform.position;
        startNode = GetNodeFromPoint(currentGridPoint);
        startNode.cost = 0;
        DijkstraNode currentNode = startNode;

        OpenList.Add(currentNode);
        int attemps=0;
        while(OpenList.Count>0)
        {
            currentNode = OpenList[0];
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);
            if (currentNode.gridPos==destinationGridPoint)
            {
                break;
            }
            foreach (DijkstraNode node in currentNode.neighbours)
            {
                if (ClosedList.Contains(node) || OpenList.Contains(node)) continue;
                int tCost = currentNode.cost + 1;
                if (tCost < node.cost)
                {
                    OpenList.Add(node);
                    node.parentNode = currentNode;
                    node.cost = tCost;
                }
            }
            OpenList = OpenList.OrderBy(x=>x.cost).ToList();
            if (attemps>1000)
            {
                Debug.LogError("Cant find Path");
                return null;
            }
            else
            {
                currentNode = OpenList[0];
            }
            attemps++;
        }
        aiPath= new List <Vector2>();
        do
        {
            aiPath.Add(ConvertGridPositionToWorldPosition(  currentNode.gridPos));
            currentNode = currentNode.parentNode;
        } while (currentNode.gridPos != currentGridPoint);
        aiPath.Reverse();
        return aiPath;
    }
    void CalculateCostForNodeAndNeighbours(DijkstraNode dijktraNode)
    {
        foreach (DijkstraNode node in dijktraNode.neighbours)
        {
            int tCost = dijktraNode.cost + 1;
            if (tCost < node.cost)
            {
                node.parentNode = dijktraNode;
                node.cost = tCost;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (dijkstraNodes==null||!isDebugActiveForCar)
        {
            return;
        }
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                if (dijkstraNodes[x, y].isObstacle)
                    Gizmos.color = Color.red;
                else Gizmos.color = Color.green;

                Gizmos.DrawWireCube(ConvertGridPositionToWorldPosition(dijkstraNodes[x, y]), new Vector3(cellSize, cellSize, cellSize));
            }
        
        
     


        foreach (DijkstraNode closedNode in ClosedList)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(closedNode), 1.0f);

        }
        //Draw the nodes that we should check
        foreach (DijkstraNode openNode in OpenList)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(ConvertGridPositionToWorldPosition(openNode), 1.0f);
        }

        Vector3 lastAIPoint = Vector3.zero;
        bool isFirstStep = true;

       // Gizmos.color = Color.black;
        Gizmos.color = Color.cyan;

        foreach (Vector2 point in aiPath)
        {
            if (!isFirstStep)
            {
                Gizmos.DrawCube(ConvertGridPositionToWorldPosition(point), new Vector3(cellSize, cellSize, cellSize));
            }

            lastAIPoint = point;

            isFirstStep = false;

        }

        //Draw start position
        Gizmos.color = Color.black;
        Gizmos.DrawCube(startPositionDebug, new Vector3(cellSize, cellSize, cellSize));

        //Draw end position
        Gizmos.color = Color.red;
        Gizmos.DrawCube(destinationPositionDebug, new Vector3(cellSize, cellSize, cellSize));

    }
}
