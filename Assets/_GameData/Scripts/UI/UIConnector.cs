using TSGameDev.Controls.MainPlayer;
using TSGameDev.UI.Tween;
using UnityEngine;

namespace TSGameDev.UI.Controls
{
    [CreateAssetMenu(fileName = "New UI Connector", menuName = "TSGameDev/Controls/New UI Connector")]
    public class UIConnector : ScriptableObject
    {
        public delegate TweenProfile GetTweenProfile();
        public GetTweenProfile GetInventoryUITween;
        public TweenState InventoryTweenState = TweenState.Close;

        public GetTweenProfile GetEquipmentUITween;
        public TweenState EquipmentTweenState = TweenState.Close;

        public GetTweenProfile GetStatsUITween;
        public TweenState StatsTweenState = TweenState.Close;

        public GetTweenProfile GetHUDIconUITween;
        public TweenState HUDIconTweenState = TweenState.Close;

        public GetTweenProfile GetTargetInventoryUITween;
        public TweenState TargetInventoryState = TweenState.Close;
        private void OnEnable()
        {
            InventoryTweenState = TweenState.Close;
            EquipmentTweenState = TweenState.Close;
            StatsTweenState = TweenState.Close;
            HUDIconTweenState = TweenState.Close;
            TargetInventoryState = TweenState.Close;
        }

        public void InventoryTween(bool forceClose = false)
        {
            if (GetInventoryUITween == null)
                return;

            TweenProfile profile = GetInventoryUITween();

            if (profile == null)
                return;

            if(forceClose || InventoryTweenState == TweenState.Open)
            {
                profile.DeactivateTween();
                InventoryTweenState= TweenState.Close;
                return;
            }

            if (InventoryTweenState == TweenState.Close)
            {
                profile.ActivateTween();
                InventoryTweenState = TweenState.Open;
                return;
            }
        }

        public void EquipmentTween()
        {
            if (GetEquipmentUITween == null)
                return;

            TweenProfile profile = GetEquipmentUITween();

            if (profile == null)
                return;

            if (EquipmentTweenState == TweenState.Close)
            {
                profile.ActivateTween();
                EquipmentTweenState = TweenState.Open;
            }
            else
            {
                profile.DeactivateTween();
                EquipmentTweenState = TweenState.Close;
            }
        }

        public void StatsTween()
        {
            if (GetStatsUITween == null)
                return;

            TweenProfile profile = GetStatsUITween();

            if (profile == null)
                return;

            if (StatsTweenState == TweenState.Close)
            {
                profile.ActivateTween();
                StatsTweenState = TweenState.Open;
            }
            else
            {
                profile.DeactivateTween();
                StatsTweenState = TweenState.Close;
            }
        }

        public void HUDIconTween()
        {
            if (GetHUDIconUITween == null)
                return;

            TweenProfile profile = GetHUDIconUITween();

            if (profile == null)
                return;

            if (HUDIconTweenState == TweenState.Close)
            {
                profile.ActivateTween();
                HUDIconTweenState = TweenState.Open;
            }
            else
            {
                profile.DeactivateTween();
                HUDIconTweenState = TweenState.Close;
            }
        }

        public void TargetInventoryTween(bool forceClose = false)
        {
            if (GetTargetInventoryUITween == null)
                return;

            TweenProfile profile = GetTargetInventoryUITween();

            if (profile == null)
                return;

            if (forceClose || TargetInventoryState == TweenState.Open)
            {
                profile.DeactivateTween();
                TargetInventoryState = TweenState.Close;
                return;
            }

            if (TargetInventoryState == TweenState.Close)
            {
                profile.ActivateTween();
                TargetInventoryState = TweenState.Open;
                return;
            }
        }

    }
}
