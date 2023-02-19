// Timer.cs - A simple timer class for managing time duration.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Timer class provides functionality for managing a time duration with a start and stop method,
// and an event handler to signal when the duration has elapsed. It also includes methods for updating the time elapsed,
// getting the current time elapsed, checking if the timer is still running, and resetting the timer.
//
// No accreditation is required but it would be highly appreciated <3

using UnityEngine;
using System;

public class Timer
{
    private readonly float duration;
    private float timeElapsed;
    public event Action OnTimerExpired;

    public Timer(float duration)
    {
        this.duration = duration;
    }

    public void Start()
    {
        timeElapsed = 0f;
    }

    public void Stop()
    {
        timeElapsed = duration;
    }

    public void Update()
    {
        timeElapsed += Time.deltaTime * Time.timeScale;

        if (timeElapsed >= duration)
        {
            OnTimerExpired?.Invoke();
        }
    }

    public float TimeElapsed
    {
        get { return timeElapsed; }
    }
    
    public bool IsRunning
    {
        get { return timeElapsed < duration; }
    }

    public void Reset()
    {
        timeElapsed = 0f;
    }
}