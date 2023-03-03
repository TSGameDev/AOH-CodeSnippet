using TSGameDev.Controls.MainPlayer;
using UnityEngine;

namespace TSGameDev.Inventories.Crafting
{
    public class CraftingStationTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject craftingStation;

        public void Cancel()
        {
            craftingStation.SetActive(false);
        }

        public bool IsToggleable()
        {
            return true;
        }

        public void OnInteract() => craftingStation.SetActive(!craftingStation.activeSelf);
    }
}

