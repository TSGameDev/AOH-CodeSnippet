using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace TSGameDev
{
    [CreateAssetMenu(fileName = "New Time Data", menuName = "TSGameDev/Day-Night Cycle/New Time Data")]
    public class TimeData : ScriptableObject
    {
        public DateTime currentTime;

        public UnityEvent OnDayStart;
        public UnityEvent OnNightStart;

        [SerializeField] private float timeMultiplier = 1f;
        [SerializeField] private float startHour = 7f;
        [SerializeField] private float dayBeginHour = 6f;
        [SerializeField] private float nightBeginHour = 18f;
        [SerializeField] private float maxSunlightIntensity = 2f;
        [SerializeField] private float maxMoonlightIntensity = 1f;

        [SerializeField] private Color dayAmbientLight;
        [SerializeField] private Color nightAmbientLight;
        [SerializeField] private AnimationCurve lightChangeCurve;

        private TimeSpan _DayBeginTime;
        private TimeSpan _NightBeginTime;

        public float GetTimeMultiplier() => timeMultiplier;
        public float GetMaxSunIntensity() => maxSunlightIntensity;
        public float GetMaxMoonIntensity() => maxMoonlightIntensity;
        public float GetDayBeginHour() => dayBeginHour;
        public float GetNightBeginHour() => nightBeginHour;
        public TimeSpan GetDayBeginTime() => _DayBeginTime;
        public TimeSpan GetNightBeginTime() => _NightBeginTime;
        public Color GetDayAmbientLight() => dayAmbientLight;
        public Color GetNightAmbientLight() => nightAmbientLight;
        public AnimationCurve GetAmbientLightAnimCurve() => lightChangeCurve;

        public void Initialisation()
        {
            currentTime = DateTime.Now + TimeSpan.FromHours(startHour);
            _DayBeginTime = TimeSpan.FromHours(dayBeginHour);
            _NightBeginTime = TimeSpan.FromHours(nightBeginHour);
        }

    }
}
