using System.Collections.Generic;
using UnityEngine;
using TSGameDev.Inventories;
using Sirenix.OdinInspector;
using static TSGameDev.Inventories.Inventory;

namespace TSGameDev.Core.Shop
{
    //Script to be placed on a shop tab to indicate what can be bought or sold to this specific section
    public class Shop : MonoBehaviour
    {
        [Tooltip("The specific tabs inventory, either items to buy or the items that can be sold to the shop.")]
        [SerializeField] Inventory shopInventory;
        [Tooltip("The item card to be used for items in this shop section. The card determines functionality liek buying selling buying back and the currency used.")]
        [SerializeField] GameObject shopCard;
        [Tooltip("The parent for all the shopcards.")]
        [SerializeField] Transform content;
        [Tooltip("The relivant time data for this shop")]
        [SerializeField] TimeData timeData;
        [Tooltip("Bool for if the shop should have its inventory updated on sunrise")]
        [SerializeField] bool updateShopInventory = false;
        [Tooltip("Bool for if this shop is a shop for selling player items")]
        [SerializeField] bool shopSelling = false;

        [Space(10)]

        [SerializeField] InventoryItem[] shopCatalogue;
        
        private List<InventorySlot> sellingSlots = new();

        //Cache for all the created shopcards. An object pool.
        private List<GameObject> _ShopCardCache = new();


        public void Initialisation()
        {
            if(!shopSelling)
            {
                //Create to shop when enabled and then assign the creation function to the event called on the inventories update event.
                UpdateShopCards();
                shopInventory.OnInventoryUpdated += UpdateShopCards;

                if (updateShopInventory && !shopSelling)
                    timeData.OnDayStart.AddListener(PopulateInventory);
            }
            else
            {
                PopulateSellingInventory();
                shopInventory.OnInventoryUpdated += PopulateSellingInventory;
            }
        }

        //Function to create and update all the cards within the shop to correctly display inventory data.
        private void UpdateShopCards() => CardSpawner.UpdateCards(_ShopCardCache, shopInventory.GetInventory(), shopCard, content);

        private void UpdateShopSellingCards() => CardSpawner.UpdateCards(_ShopCardCache, sellingSlots.ToArray(), shopCard, content);

        private void PopulateInventory()
        {
            shopInventory.ClearInventory();
            foreach(InventoryItem item in shopCatalogue)
            {
                int shopRoll = Random.Range(0, 100);
                double itemShopChance = item.GetShopChance();
                if(shopRoll <= itemShopChance)
                {
                    int shopAmount = Random.Range(0, item.GetMaxShopAmount());
                    shopInventory.AddToFirstEmptySlot(item, shopAmount);
                }
            }
        }

        private void PopulateSellingInventory()
        {
            sellingSlots.Clear();
            foreach(InventorySlot slot in shopInventory.GetInventory())
            {
                if (slot.item == null)
                    continue;
                else if(slot.item.GetSellable())
                {
                    sellingSlots.Add(slot);
                }
            }
            UpdateShopSellingCards();
        }
    }
}
