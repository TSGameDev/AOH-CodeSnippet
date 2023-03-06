using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Core.AI
{
    [Serializable]
    public struct AIStats
    {
        [Tooltip("The max health of this entity.")]
        public int health;
        [Tooltip("The speed of this entity as a percentage decimal. 1 is normal, 0.5 is half speed. Set to the animator.speed as entity uses root motion.")]
        public int speed;
        [Tooltip("The damage dealt by this entity per hit.")]
        public int damage;
        [Tooltip("The Range that this entity can see the player")]
        public float visualRange;
        [Tooltip("The totle angle of the AIs attack range, I.E. 90 will be 45 degree right and left of the AI")]
        public float attackAngle;
        [Tooltip("The Range that this entities attacks reaches, this shoudl be greater than the attack trigger range")]
        public float attackRange;
        [Tooltip("The Range that will trigger the entity to attack")]
        public float attackTriggerRange;

        public AIStats(int _Health, int _Speed, int _Damage, float _VisualRange, float _AttackAngle, float _AttackRange, float _AttackTriggerRange)
        {
            health = _Health;
            speed = _Speed;
            damage = _Damage;
            visualRange = _VisualRange;
            attackAngle = _AttackAngle;
            attackRange = _AttackRange;
            attackTriggerRange = _AttackTriggerRange;
        }
    }

    [CreateAssetMenu(fileName = "New AI Data", menuName = "TSGameDev/AI/AI Data")]
    public class AIData : ScriptableObject
    {
        //Struct for all the ai stats
        [SerializeField] private AIStats aiStats;

        public AIStats GetAIStats() => aiStats;
    }
}
