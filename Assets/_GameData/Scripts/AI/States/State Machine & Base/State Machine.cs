using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Core.AI
{
    public class StateMachine    
    {
        private State _currentState;
        public State CurrentState
        {
            get => _currentState;
            set 
            {
                if (_currentState != null)
                    _currentState.Exit();

                _currentState = value;

                if (_currentState != null)
                    _currentState.Enter();
            }
        }
    }
}
