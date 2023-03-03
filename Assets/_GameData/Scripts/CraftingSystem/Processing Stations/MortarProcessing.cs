using TSGameDev.UI.Inventories.Crafting;
using UnityEngine;

namespace TSGameDev.Inventories.Crafting
{
    public class MortarProcessing : MonoBehaviour
    {
        [SerializeField] CraftingSlotUI reagentSlot;
        [SerializeField] CraftingResultSlotUI resultSlot1;
        [SerializeField] CraftingResultSlotUI resultSlot2;

        public void ProcessIngredient()
        {
            ReagentItem reagentItem = reagentSlot.GetItem() as ReagentItem;
            if (reagentItem == null)
                return;

            InventoryItem processedItemPrimary = reagentItem.GetMortarPrimaryResult();
            InventoryItem processItemSecondary = reagentItem.GetMortarSecondaryResult();

            if(processedItemPrimary != null)
            {
                resultSlot1.AddItem(processedItemPrimary, Random.Range(1, 5));
                reagentSlot.RemoveItems(1);
            }

            if(processItemSecondary != null)
            {
                resultSlot2.AddItem(processItemSecondary, Random.Range(1, 5));
            }
        }

    }

}
