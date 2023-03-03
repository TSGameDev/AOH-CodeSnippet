using UnityEngine;
using TSGameDev.Inventories;
using TSGameDev.UI.Inventories.ToolTips;
using TSGameDev.UI.Inventories.Dragging;
using static TSGameDev.Inventories.Inventory;

namespace TSGameDev.UI.Inventories.Crafting
{
    public class CraftingSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        #region Serialized Variables

        //Reference to the child InventoryItemIcon that is set from within the prefab.
        [SerializeField] InventoryItemIcon icon = null;
        //Bool to allow the crafting slot to accept Reagent Items
        [SerializeField] bool allowReagents = false;
        //Bool to allow the crafting slot to accept Base Items
        [SerializeField] bool allowBases = false;

        #endregion

        #region Private Variables

        private InventorySlot storedItem;

        #endregion

        #region Public Functions

        public void AddItems(InventoryItem item, int number)
        {
            ReagentItem reagentItem = allowReagents ? item as ReagentItem : null;
            BaseItem baseItem = allowBases ? item as BaseItem : null;
            PotionItem potionitem = allowBases && baseItem == null ? item as PotionItem : null;

            if (reagentItem != null || baseItem != null || potionitem != null)
            {
                storedItem.item = item;
                storedItem.number = number;
                icon.SetItem(item, number);
            }
        }

        public InventoryItem GetItem()
        {
            return storedItem.item;
        }

        public int GetNumber()
        {
            return storedItem.number;
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return 1;
        }

        public void RemoveItems(int number)
        {
            storedItem.item = null;
            storedItem.number = 0;
            icon.SetItem(null, 0);
        }

#endregion

    }
}

