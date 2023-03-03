using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Core.AI
{
    /// <summary>
    /// Object that tells the state machine what actions this AI can take.
    /// </summary>
    [CreateAssetMenu(fileName = "New AI Brain", menuName = "TSGameDev/AI/AI Brain")]
    public class AIBrain : ScriptableObject
    {
        //Bools or conditionals that the state machine checks before performing actions you don't want all AI using the state machine to perform.

        #region State Conditionals

        [SerializeField] private bool isWonder = false;
        public bool GetWonder() => isWonder;


        #endregion

        #region Animation Hashes

        public readonly int speedHash = Animator.StringToHash("Speed");
        public readonly int attackHash = Animator.StringToHash("Attack");
        public readonly int deadHash = Animator.StringToHash("IsDead");

        #endregion
    }
}
