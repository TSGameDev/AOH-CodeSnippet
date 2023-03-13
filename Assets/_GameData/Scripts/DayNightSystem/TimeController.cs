using System;
using TMPro;
using UnityEngine;

namespace TSGameDev
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private TimeData timeData;
        [SerializeField] private TextMeshProUGUI timeDisplay;

        [SerializeField] private Light sunLight;
        [SerializeField] private Light moonLight;

        private bool _DayStartTrigger = false, _NightStartTrigger = false;

        private void Start()
        {
            timeData.Initialisation();
        }

        private void Update()
        {
            UpdateTimeOfDay();
            if(sunLight != null || moonLight != null)
            {
                RotateSun();
                UpdateLightSettings();
            }
        }

        private void UpdateTimeOfDay()
        {
            timeData.currentTime = timeData.currentTime.AddSeconds(Time.deltaTime * timeData.GetTimeMultiplier());

            if(timeDisplay != null)
                timeDisplay.text = timeData.currentTime.ToString("HH:mm");

            if(timeData.currentTime.Hour == timeData.GetDayBeginHour() && !_DayStartTrigger)
            {
                timeData.OnDayStart.Invoke();
                _DayStartTrigger = true;
                _NightStartTrigger = false;
            }

            if (timeData.currentTime.Hour == timeData.GetNightBeginHour() && !_NightStartTrigger)
            {
                timeData.OnNightStart.Invoke();
                _DayStartTrigger = false;
                _NightStartTrigger = true;
            }
        }

        private void RotateSun()
        {
            float _SunRotation;
            TimeSpan _CurrentTimeOfDay = timeData.currentTime.TimeOfDay;
            TimeSpan _NightBeginTime = timeData.GetNightBeginTime();
            TimeSpan _DayBeginTime = timeData.GetDayBeginTime();

            if (_CurrentTimeOfDay > _DayBeginTime && _CurrentTimeOfDay < _NightBeginTime)
            {
                TimeSpan _SunriseToSunsetDuration = CalculateTimeDifferent(_DayBeginTime, _NightBeginTime);
                TimeSpan _TimeSinceSunrise = CalculateTimeDifferent(_DayBeginTime, _CurrentTimeOfDay);

                double _PercentageOfDay = _TimeSinceSunrise.TotalMinutes / _SunriseToSunsetDuration.TotalMinutes;
                _SunRotation = Mathf.Lerp(0, 180, (float)_PercentageOfDay);
            }
            else
            {
                TimeSpan _SunsetToSunriseDuration = CalculateTimeDifferent(_NightBeginTime, _DayBeginTime);
                TimeSpan _TimeSinceSunset = CalculateTimeDifferent(_NightBeginTime, _CurrentTimeOfDay);

                double _PercentageOfNight = _TimeSinceSunset.TotalMinutes / _SunsetToSunriseDuration.TotalMinutes;
                _SunRotation = Mathf.Lerp(180, 360, (float)_PercentageOfNight);
            }

            sunLight.transform.rotation = Quaternion.AngleAxis(_SunRotation, Vector3.right);
        }

        private TimeSpan CalculateTimeDifferent(TimeSpan _FromTime, TimeSpan _ToTime)
        {
            TimeSpan _Difference = _ToTime - _FromTime;

            if (_Difference.TotalSeconds < 0)
                _Difference += TimeSpan.FromHours(24);

            return _Difference;
        }

        private void UpdateLightSettings()
        {
            float _DotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
            float _EvaluatedDotProduct = timeData.GetAmbientLightAnimCurve().Evaluate(_DotProduct);
            sunLight.intensity = Mathf.Lerp(0, timeData.GetMaxSunIntensity(), _EvaluatedDotProduct);
            moonLight.intensity = Mathf.Lerp(timeData.GetMaxMoonIntensity(), 0, _EvaluatedDotProduct);
            RenderSettings.ambientLight = Color.Lerp(timeData.GetNightAmbientLight(), timeData.GetDayAmbientLight(), _EvaluatedDotProduct);
        }
    }
}
