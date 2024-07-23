using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveData: MonoBehaviour
{
    public Vector2 curveEndDirection;
    public Vector2 curveStartDirection;
    public float curveRadius;
    public Transform rad;
    public float angle;
    private void Awake()
    {
        curveRadius = Vector2.Distance(transform.position, rad.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        curveRadius = Vector2.Distance(transform.position, rad.position);
        angle = Vector2.SignedAngle(curveStartDirection, curveEndDirection);
        Gizmos.DrawWireSphere(transform.position, curveRadius);

    }
}
