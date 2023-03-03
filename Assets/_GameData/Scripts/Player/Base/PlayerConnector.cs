using UnityEngine;
using TSGameDev.UI.Tween;
using System;

namespace TSGameDev.Controls.MainPlayer
{
    public enum TweenState
    { 
        Open,
        Close
    }

    [CreateAssetMenu(fileName = "New Player Connector", menuName = "TSGameDev/Controls/New Player Connector")]
    public class PlayerConnector : ScriptableObject
    {
        public Action playerInteraction;
    }
}
