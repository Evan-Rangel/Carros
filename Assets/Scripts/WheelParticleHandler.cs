using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    #region Local Variables
    float particleEmissionRate = 0f;
    #endregion

    #region Components
    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;
    #endregion

    #region Unity Default Functions
    private void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        particleSystemSmoke = GetComponent<ParticleSystem>();

        particleSystemEmissionModule = particleSystemSmoke.emission;

        //Set it to zero emission
        particleSystemEmissionModule.rateOverTime = 0;
    }

    private void Start()
    {

    }

    private void Update()
    {
        //Reduce the particles over time.
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if (topDownCarController.isTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            //If the car tires are screeching then we'll emitt smoke. If the player
            if (isBraking)
            {
                particleEmissionRate = 30;
            }
            //If the player is drifting we'll emitt smoke based on how much the player drift
            else
            {
                particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
            }
        }

    }

    private void FixedUpdate()
    {

    }
    #endregion

    #region Functions

    #endregion
}
