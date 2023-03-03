using TSGameDev.Inventories;
using TSGameDev.UI.Inventories;
using TSGameDev.UI.Inventories.ToolTips;
using TSGameDev.UI.Inventories.Dragging;
using static TSGameDev.Inventories.Inventory;
using UnityEngine;

namespace TSGameDev.UI.Inventories.Crafting
{
    public class CraftingResultSlotUI : MonoBehaviour, IItemHolder, IDragSource<InventoryItem>
    {
        #region Serialized Variables

        //Reference to the child InventoryItemIcon that is set from within the prefab.
        [SerializeField] InventoryItemIcon icon = null;

        #endregion

        #region Private Variables

        private InventorySlot storedItem;

        #endregion

        #region Public Functions

        public void AddItem(InventoryItem item, int number)
        {
            storedItem.item = item;
            storedItem.number = number;
            icon.SetItem(item, number);
        }

        public InventoryItem GetItem()
        {
            return storedItem.item;
        }

        public int GetNumber()
        {
            if (!storedItem.item.IsStackable())
                return 1;

            return storedItem.number;
        }

        public void RemoveItems(int number)
        {
            if(storedItem.number > 1)
            {
                storedItem.number -= number;
                icon.SetNumber(storedItem.number);
            }

            if(storedItem.number <= 1)
            {
                storedItem.item = null;
                storedItem.number = 0;
                icon.SetItem(null, 0);
            }
        }

        #endregion
    }
}


