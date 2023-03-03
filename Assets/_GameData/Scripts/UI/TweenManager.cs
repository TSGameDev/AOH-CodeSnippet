using TSGameDev.UI.Controls;
using UnityEngine;

namespace TSGameDev.UI.Tween
{
    public class TweenManager : MonoBehaviour
    {
        [SerializeField] UIConnector uiConnector;

        [SerializeField] TweenProfile inventoryTweenProfile;
        [SerializeField] TweenProfile equipmentTweenProfile;
        [SerializeField] TweenProfile statsTweenProfile;
        [SerializeField] TweenProfile hudIconTweenProfile;
        [SerializeField] TweenProfile targetInventoryTweenProfile;

        private void Awake()
        {
            uiConnector.GetInventoryUITween = () =>
            {
                if (inventoryTweenProfile == null)
                    return null;

                return inventoryTweenProfile;
            };

            uiConnector.GetEquipmentUITween = () =>
            {
                if (equipmentTweenProfile == null)
                    return null;

                return equipmentTweenProfile;
            };

            uiConnector.GetStatsUITween = () =>
            {
                if (statsTweenProfile == null)
                    return null;

                return statsTweenProfile;
            };

            uiConnector.GetHUDIconUITween = () =>
            {
                if (hudIconTweenProfile == null)
                    return null;

                return hudIconTweenProfile;
            };

            uiConnector.GetTargetInventoryUITween = () =>
            {
                if(targetInventoryTweenProfile == null)
                    return null;
                return targetInventoryTweenProfile;
            };

        }
    }
}
