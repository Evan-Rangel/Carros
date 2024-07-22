using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecisionTree : MonoBehaviour
{
    CarAIHandler aiHandler;
    TopDownCarController topDownCarController;
    DecisionNode firstNode;
    [SerializeField] Transform[] raycastPositions;

    [SerializeField] float raycastDistance;
    List<Vector2> directions;
    public Vector2 pos;
    public Transform target;


    private void Awake()
    {
        directions = new List<Vector2>();
        foreach (Transform dir in raycastPositions)
        {
            directions.Add((dir.position- transform.position).normalized);
        }
        aiHandler = GetComponent<CarAIHandler>();
        topDownCarController = GetComponent<TopDownCarController>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < raycastPositions.Length; i++)
        {
            RaycastHit2D raycastHit2D =Physics2D.Raycast(raycastPositions[i].position, directions[i],raycastDistance);
            Debug.DrawRay(raycastPositions[i].position, directions[i]* raycastDistance, Color.black);
            if (raycastHit2D.collider!=null )
            {

            }
        }
    }
}

public class DecisionTreeNode
{
    protected DecisionTreeNode leftNode;
    protected DecisionTreeNode rightNode;

    //protected UnityEvent ExcecuteEvent;
}
public class DecisionNode: DecisionTreeNode
{
    
    public  DecisionTreeNode GetNextNode(
        [Tooltip("Return the leftNode if value1 its less than value2")]
        float value1, float value2)
    {
        return value1 < value2 ? leftNode : rightNode;
    }
}
public class LeafNode: DecisionTreeNode
{
    public void Result()
    { 
        
    }
}