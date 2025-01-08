using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConePerSecondTimer : MonoBehaviour
{
    [SerializeField] private float TimerDuration = 1f; // Duration between increases
    public double ConePerSecond { get; set; } // Amount to increase per second

    private float _counter; // Counter to track elapsed time

    private void Update()
    {
        if (ConePerSecond <= 0) return; // Skip processing if no CPS is set

        _counter += Time.deltaTime;

        if (_counter >= TimerDuration)
        {
            if (ConeManager.instance != null)
            {
                ConeManager.instance.SimpleConeIncrease(ConePerSecond); // Increase cones
            }
            else
            {
                Debug.LogError("ConeManager.instance is null. Ensure ConeManager exists in the scene.");
            }

            _counter -= TimerDuration; // Reset counter
        }
    }
}
