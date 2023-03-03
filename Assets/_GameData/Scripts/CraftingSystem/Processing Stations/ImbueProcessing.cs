using System.Collections.Generic;
using TSGameDev.Inventories;
using TSGameDev.UI.Inventories.Crafting;
using UnityEngine;

namespace TSGameDev.Inventories.Crafting
{
    public class ImbueProcessing : MonoBehaviour
    {
        [SerializeField] CraftingSlotUI baseSlot;
        [SerializeField] CraftingSlotUI reagentSlot;
        [SerializeField] CraftingResultSlotUI resultSlot;

        public void ProcessIngredient()
        {
            BaseItem baseItem = baseSlot.GetItem() as BaseItem;
            ReagentItem reagentItem = reagentSlot.GetItem() as ReagentItem;

            if (InventoryItem.baseLookupCache == null)
                InventoryItem.CreateItemCaches();

            ImbueProcess craftingRecipe = new(baseItem, reagentItem);

            foreach (KeyValuePair<string, BaseItem> KV in InventoryItem.baseLookupCache)
            {
                bool baseMatch = false, reagentMatch = false;
                BaseItem currentBase = KV.Value;
                ImbueProcess recipe = currentBase.GetImbueResult();

                if (craftingRecipe.baseItem == recipe.baseItem)
                    baseMatch = true;

                if (craftingRecipe.reagentItem == recipe.reagentItem)
                    reagentMatch = true;

                if (baseMatch && reagentMatch)
                {
                    baseSlot.RemoveItems(1);
                    reagentSlot.RemoveItems(1);
                    resultSlot.AddItem(currentBase, Random.Range(1, 5));
                    return;
                }
            }
        }
    }

}
