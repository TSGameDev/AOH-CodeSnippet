using System;
using UnityEngine;

namespace TSGameDev.Core.Effects
{
    [Serializable]
    public struct PlayerStatsData
    {
        public int health;
        public bool isHittable;

        public PlayerStatsData(int _Health)
        {
            health= _Health;
            isHittable= true;
        }
    }

    [CreateAssetMenu(fileName = "New Player Stats", menuName = "TSGameDev/Player/Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField] private PlayerStatsData playerStats;

        public PlayerStatsData GetBasePlayerStats() => playerStats;
    }
}
