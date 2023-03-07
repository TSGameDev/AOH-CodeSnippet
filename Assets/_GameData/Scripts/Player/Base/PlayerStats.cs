using System;
using UnityEngine;

namespace TSGameDev.Core.Effects
{
    [Serializable]
    public struct PlayerStatsData
    {
        public int health;
        private bool _IsHittable;

        public PlayerStatsData(int _Health)
        {
            health= _Health;
            _IsHittable= true;
        }

        public void ReduceHealth(int _Damage)
        {
            if(_IsHittable)
            {
                health -= _Damage;
                _IsHittable= false;
            }
        }

        public void SetIsHittable(bool _IsHittable) => this._IsHittable= _IsHittable;
    }

    [CreateAssetMenu(fileName = "New Player Stats", menuName = "TSGameDev/Player/Player Stats")]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField] private PlayerStatsData playerStats;

        public PlayerStatsData GetBasePlayerStats() => playerStats;
    }
}
