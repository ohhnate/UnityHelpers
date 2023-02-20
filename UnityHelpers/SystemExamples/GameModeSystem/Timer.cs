// Timer.cs - A simple timer class for managing time duration.
// Version 1.0.1
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

namespace UnityHelpers
{
    public class Timer
    {
        private readonly float _duration;
        private float _timeElapsed;
        public event Action OnTimerExpired;
        
        public TimeSpan TimeElapsed => TimeSpan.FromSeconds(_timeElapsed);

        public bool IsRunning => _timeElapsed < _duration;

        public Timer(TimeSpan duration)
        {
            _duration = (float)duration.TotalSeconds;
        }

        public void Start()
        {
            _timeElapsed = 0f;
        }

        public void Stop()
        {
            _timeElapsed = _duration;
        }

        public void Update()
        {
            _timeElapsed += Time.deltaTime * Time.timeScale;

            if (_timeElapsed >= _duration)
            {
                OnTimerExpired?.Invoke();
            }
        }

        public void Reset()
        {
            _timeElapsed = 0f;
        }
    }
}