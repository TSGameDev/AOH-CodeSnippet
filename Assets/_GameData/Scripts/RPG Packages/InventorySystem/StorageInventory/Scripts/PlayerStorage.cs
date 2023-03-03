using TMPro;
using TSGameDev.Controls.MainPlayer;
using TSGameDev.UI.Controls;
using TSGameDev.UI.Inventories;
using UnityEngine;

namespace TSGameDev.Inventories
{
    public class PlayerStorage : MonoBehaviour, IInteractable
    {
        [SerializeField] private string storageName;
        [SerializeField] private TMP_Text storageNameTxt;

        [SerializeField] private Inventory storageInventory;
        [SerializeField] private InventoryUI storageInventoryUI;
        [SerializeField] private UIConnector uIConnector;

        public void Cancel()
        {
            uIConnector.TargetInventoryTween(true);
            uIConnector.InventoryTween(true);
        }

        public bool IsToggleable() => true;

        public void OnInteract()
        {
            if(storageInventoryUI.GetInventory() != storageInventory)
            {
                storageInventoryUI.SetInventory(storageInventory);
                storageInventoryUI.Redraw();
                storageNameTxt.text = storageName;
            }

            uIConnector.TargetInventoryTween();
            uIConnector.InventoryTween();
        }
    }
}
