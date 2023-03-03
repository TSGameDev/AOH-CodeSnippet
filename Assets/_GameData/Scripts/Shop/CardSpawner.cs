using System.Collections.Generic;
using UnityEngine;
using static TSGameDev.Inventories.Inventory;

namespace TSGameDev.Core
{
    //Interface contract for shop cards to impliment to make sure they have the initialisation function
    public interface ICard
    {
        public void Initialisation(InventorySlot slot, int slotIndex);
    }

    //Static class that can be called from anywhere to spawn cards under the content and update all the cards from an object pool.
    public static class CardSpawner
    {
        //Updates all the cards witin the object pool, either calling their initialisation function to correctly display data or deactivating them if they are not needed.
        public static void UpdateCards(List<GameObject> objCache, InventorySlot[] inventory, GameObject cardPrefab, Transform contentObj)
        {
            //If the inventory has more items than spawned cards, spawns more cards.
            if (inventory.Length > objCache.Count)
            {
                int _CardsToSpawn = inventory.Length - objCache.Count;
                CreateCards(objCache, cardPrefab, contentObj, _CardsToSpawn);
            }

            //For each card within the object pool, see if the card index matchs to an item and call its initialisation method. If the index is higher than the count of the ivnentory just deactivate all remaining card objects.
            for (int i = 0; i <= objCache.Count - 1; i++)
            {
                if (objCache[i] == null)
                    continue;

                objCache[i].SetActive(false);
                if (i <= (inventory.Length - 1) && inventory[i].item != null)
                {
                    objCache[i].GetComponent<ICard>().Initialisation(inventory[i], i);
                }
            }
        }

        //Creates new card objects, deactivating them and storing them within the object pool.
        private static void CreateCards(List<GameObject> objCache, GameObject cardPrefab, Transform contentObj, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject newItemCard = GameObject.Instantiate(cardPrefab, contentObj);
                newItemCard.SetActive(false);
                objCache.Add(newItemCard);
            }
        }
    }
}
