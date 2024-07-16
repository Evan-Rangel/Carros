using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    #region Components
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;
    #endregion

    #region Unity Default Functions
    private void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        trailRenderer = GetComponent<TrailRenderer>();  

        //Set the trail renderer to not emit in the start
        trailRenderer.emitting = false;
    }

    private void Start()
    {

    }

    private void Update()
    {
        //If the car tires are screeching then we'll emitt a trail
        if (topDownCarController.isTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }
        else 
        {
            trailRenderer.emitting = false;
        }
    }

    private void FixedUpdate()
    {

    }
    #endregion

    #region Functions

    #endregion
}
