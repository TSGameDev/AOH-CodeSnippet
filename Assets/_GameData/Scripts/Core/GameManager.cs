using UnityEngine;
using CodeMonkey.Utils;

namespace TSGameDev.Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            TimeTickSystem.Create();
        }
    }
}

