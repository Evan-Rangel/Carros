using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    #region Local Variables

    #endregion

    #region Components
    TopDownCarController topDownCarController;
    #endregion

    #region Unity Default Functions
    private void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        topDownCarController.SetInputVector(inputVector);

    }

    private void FixedUpdate()
    {

    }
    #endregion

    #region Functions

    #endregion
}
