using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    #region Car Lap
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();
    #endregion

    #region Local Variables

    #endregion

    #region Components

    #endregion

    #region Unity Default Functions
    private void Awake()
    {

    }

    private void Start()
    {
        //Get all Car lap counters in the scene
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        //Store the lap counters in a list
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        //Hook up the pased checkpoint event
        foreach (CarLapCounter lapCounters in carLapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;
        }
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

    }
    #endregion

    #region Functions
    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        //Sort the cars position first based on how many checkpoints they have passed, more is always better. Then sort on time where shorter time is better
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckPoint()).ToList();

        //Get the cars position
        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        //Tell the lap counter which position the car has
        carLapCounter.SetCarPosition(carPosition);
    }
    #endregion
}
