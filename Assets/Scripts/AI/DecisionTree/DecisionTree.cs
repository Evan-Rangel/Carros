using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    [SerializeField] Transform[] rayPoints;

    CarAIHandler aiHandler;
    public LayerMask ignoreLayer;
    Vector2 ctarget;
    int waypointCounter;
    bool[] canSeeWaypoint = {true, true, true,true };
    [SerializeField] bool isDebugActiveForCar = true;

    private void Awake()
    {
        aiHandler = GetComponent<CarAIHandler>();
    }
    public List<Vector2> PathFinding(Vector2 target)
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            canSeeWaypoint[i] = false;
        }
        waypointCounter = 0;
        ctarget = target;
        RaycastHit2D rightHit;
        RaycastHit2D leftHit;
        RaycastHit2D backtHit;
        rightHit = Physics2D.Raycast(rayPoints[1].position, transform.right, 10, ~ignoreLayer);
        leftHit = Physics2D.Raycast(rayPoints[2].position, transform.right*-1, 10, ~ignoreLayer);
        backtHit = Physics2D.Raycast(rayPoints[3].position, transform.up*-1, 10, ~ignoreLayer);
        StartCoroutine(RayToWaypoint());
        List<Vector2> result = new List<Vector2>();
        if (rightHit.collider == null)
        {
            result.Add(rayPoints[1].position+transform.right*4);
        }
        else
        if (leftHit.collider == null)
        {   
            result.Add(rayPoints[2].position - (transform.right * 4));
        }
        else
        if (rightHit.distance < leftHit.distance&& backtHit.distance< leftHit.distance)
        {
            result.Add(leftHit.point);
        }
        else 
        if(rightHit.distance > backtHit.distance)
        {
            result.Add(rightHit.point);
        }else
        {
            result.Add(backtHit.point);
        }
        StartCoroutine(CheckIfCanReachWaypoint());
        return result;
    }

    IEnumerator RayToWaypoint()
    {
        while (waypointCounter<2)
        {
            CheckRaycast();
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
    void CheckRaycast()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayPoints[i].position, ctarget - (Vector2)rayPoints[i].position, Vector2.Distance(rayPoints[i].position, ctarget), ~ignoreLayer );
            
            if (hit.collider.CompareTag("Waypoint") && !canSeeWaypoint[i])
            {
                Debug.Log(i + " See Waypoint");
                canSeeWaypoint[i] = true;
                waypointCounter++;
            }
        }
    }
    IEnumerator CheckIfCanReachWaypoint()
    {
        yield return new WaitUntil(() => waypointCounter>=2);
        Debug.Log("Find Waypoint");
        aiHandler.RemoveTemporaryWaypointZero();
        yield break;
    }
    private void OnDrawGizmos()
    {
        if (!isDebugActiveForCar)
            return;
        foreach (Transform _pos in rayPoints)
        {
            Gizmos.color=Color.yellow;
            Gizmos.DrawLine(_pos.position, ctarget);
        }
    }

}

