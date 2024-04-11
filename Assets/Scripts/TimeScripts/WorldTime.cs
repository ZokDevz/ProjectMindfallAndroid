using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField]
        private float _dayLength; // in seconds

        private TimeSpan _currentTime;

        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

        private void Start()
        {
           StartCoroutine(AddMinute());
        }

        private IEnumerator AddMinute()
        {
            _currentTime += TimeSpan.FromMinutes(0.06667); //0.06667 means playback slow mo, 0 means time stop, 1 means normal time in second, and 2 means double speed.
            WorldTimeChanged?.Invoke(this, _currentTime);
            yield return new WaitForSeconds(_minuteLength);
            StartCoroutine(AddMinute());
        }

    }
}