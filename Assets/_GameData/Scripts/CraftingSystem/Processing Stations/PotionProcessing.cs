using System.Collections.Generic;
using TSGameDev.UI.Inventories.Crafting;
using UnityEngine;

namespace TSGameDev.Inventories.Crafting
{
    public class PotionProcessing : MonoBehaviour
    {
        [SerializeField] CraftingSlotUI baseCraftingSlot;
        [SerializeField] CraftingSlotUI reagentPrimaryCraftingSlot;
        [SerializeField] CraftingSlotUI reagentSecondaryCraftingSlot;

        [SerializeField] CraftingResultSlotUI resultSlot;

        public void ProcessIngredient()
        {
            BaseItem baseItem = baseCraftingSlot.GetItem() as BaseItem;
            PotionItem potionItem = baseCraftingSlot.GetItem() as PotionItem;
            ReagentItem reagentPrimary = reagentPrimaryCraftingSlot.GetItem() as ReagentItem;
            ReagentItem reagentSecondary = reagentPrimaryCraftingSlot.GetItem() as ReagentItem;

            if (InventoryItem.potionLookupCache == null)
                InventoryItem.CreateItemCaches();

            PotionRecipe craftinRecipe = new();
            if (baseItem != null)
                craftinRecipe = new(baseItem, reagentPrimary, reagentSecondary);
            else
                craftinRecipe = new(potionItem, reagentPrimary, reagentSecondary);

            foreach(KeyValuePair<string, PotionItem> KV in InventoryItem.potionLookupCache)
            {
                bool baseMatch = false, reagentPriMatch = false, reagentSecMatch = false;
                PotionItem potion = KV.Value;
                PotionRecipe recipe = potion.GetPotionRecipe();
                
                if(craftinRecipe.baseItem == null)
                {
                    if(craftinRecipe.potionItem == recipe.potionItem)
                        baseMatch= true;
                }
                else
                {
                    if (craftinRecipe.baseItem == recipe.baseItem)
                        baseMatch = true;
                }

                if (craftinRecipe.reagentItemPrimary == recipe.reagentItemPrimary || craftinRecipe.reagentItemPrimary == recipe.reagentItemSecondary)
                    reagentPriMatch = true;

                if (craftinRecipe.reagentItemSecondary == recipe.reagentItemPrimary || craftinRecipe.reagentItemSecondary == recipe.reagentItemSecondary)
                    reagentSecMatch = true;

                if (baseMatch && reagentPriMatch && reagentSecMatch)
                {
                    baseCraftingSlot.RemoveItems(1);
                    reagentPrimaryCraftingSlot.RemoveItems(1);
                    reagentSecondaryCraftingSlot.RemoveItems(1);

                    resultSlot.AddItem(potion, Random.Range(1,5));
                }
            }
        }
    }
}

