using TSGameDev.Inventories;
using TSGameDev.UI.Inventories.Crafting;
using UnityEngine;

namespace TSGameDev.Inventories.Crafting
{
    public class BlenderProcessing : MonoBehaviour
    {
        [SerializeField] CraftingSlotUI reagentSlot;
        [SerializeField] CraftingResultSlotUI resultSlot;

        public void ProcessIndredient()
        {
            ReagentItem reagentItem = reagentSlot.GetItem() as ReagentItem;
            InventoryItem processedItem = reagentItem.GetBlendingResult();

            if (processedItem != null)
            {
                resultSlot.AddItem(processedItem, Random.Range(1, 5));
                reagentSlot.RemoveItems(1);
            }

        }
    }
}

