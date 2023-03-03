using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TSGameDev.Inventories;
using static TSGameDev.Inventories.Inventory;

namespace TSGameDev.Core.Shop
{
    [Serializable]
    enum CardType
    {
        BuyCard,
        SellCard,
        BuyBackCard,
        QuotaCard,
        DonationCard
    }

    public class ShopCard : MonoBehaviour, ICard
    {
        [SerializeField] CardType cardType;
        [SerializeField] Image itemImage;
        [SerializeField] TextMeshProUGUI itemName, itemAmount, itemPriceBronze, itemPriceSilver, itemPriceGold;
        [SerializeField] Inventory playerInventory, shopInventory;
        
        private int _InventorySlotCache;
        private Button _InteractionButton;

        public void Initialisation(InventorySlot slot, int slotIndex)
        {
            #region General Shop Card Functionality

            if (_InteractionButton == null)
                _InteractionButton = GetComponent<Button>();
            _InteractionButton.onClick.RemoveAllListeners();

            gameObject.SetActive(true);
            _InventorySlotCache = slotIndex;

            itemImage.sprite = slot.item.GetIcon();
            itemName.text = slot.item.name;
            itemAmount.text = slot.number.ToString();

            #endregion

            CurrencySet itemSellPrice = slot.item.GetItemSellPrice();

            int ItemSellBronze = itemSellPrice.bronze;
            int ItemSellSilver = itemSellPrice.silver;
            int ItemSellGold = itemSellPrice.gold;

            CurrencySet ItemBuyPrice = slot.item.GetItemBuyPrice();

            int ItemBuyBronze = ItemBuyPrice.bronze;
            int ItemBuySilver = ItemBuyPrice.silver;
            int ItemBuyGold = ItemBuyPrice.gold;

            switch (cardType)
            {
                case CardType.BuyCard:
                    itemPriceBronze.text = $"{ItemBuyBronze}";
                    itemPriceSilver.text = $"{ItemBuySilver}";
                    itemPriceGold.text = $"{ItemBuyGold}";
                    _InteractionButton.onClick.AddListener(() => 
                    {
                        BuyCard(slot, slotIndex);
                    });
                    break;
                case CardType.SellCard:
                    itemPriceBronze.text = $"{ItemSellBronze}";
                    itemPriceSilver.text = $"{ItemSellSilver}";
                    itemPriceGold.text = $"{ItemSellGold}";
                    _InteractionButton.onClick.AddListener(() =>
                    {
                        SellCard(slot, slotIndex);
                    });
                    break;
                case CardType.BuyBackCard:
                    itemPriceBronze.text = $"{ItemSellBronze}";
                    itemPriceSilver.text = $"{ItemSellSilver}";
                    itemPriceGold.text = $"{ItemSellGold}";
                    _InteractionButton.onClick.AddListener(() =>
                    {
                        BuyBackCard(slot, slotIndex);
                    });
                    break;
                case CardType.QuotaCard:
                    itemPriceBronze.text = $"{ItemBuyBronze}";
                    itemPriceSilver.text = $"{ItemBuySilver}";
                    itemPriceGold.text = $"{ItemBuyGold}";
                    _InteractionButton.onClick.AddListener(() =>
                    {
                        QuotaCard(slot, slotIndex);
                    });
                    break;
                case CardType.DonationCard:
                    itemPriceBronze.text = $"N/A";
                    itemPriceSilver.text = $"N/A";
                    itemPriceGold.text = $"N/A";
                    _InteractionButton.onClick.AddListener(() =>
                    {
                        DonationCard(slot, slotIndex);
                    });
                    break;

            }

        }

        private void BuyCard(InventorySlot slot, int slotIndex)
        {
            if(playerInventory.RemoveInventoryCurrency(slot.item.GetItemBuyPrice()))
            {
                playerInventory.AddToFirstEmptySlot(slot.item, 1);
                shopInventory.RemoveItem(slot.item, 1);
            }
        }

        private void SellCard(InventorySlot slot, int slotIndex)
        {
            if(playerInventory.RemoveItem(slot.item, 1))
            {
                shopInventory.AddToFirstEmptySlot(slot.item, 1);
                playerInventory.AddInventoryCurrency(slot.item.GetItemSellPrice());
            }
        }

        private void BuyBackCard(InventorySlot slot, int slotIndex)
        {
            if(playerInventory.RemoveInventoryCurrency(slot.item.GetItemSellPrice()))
            {
                playerInventory.AddToFirstEmptySlot(slot.item, 1);
                shopInventory.RemoveItem(slot.item, 1);
            }
        }

        private void QuotaCard(InventorySlot slot, int slotIndex)
        {
            if(playerInventory.RemoveItem(slot.item, 1))
            {
                shopInventory.RemoveItem(slot.item, 1);
                playerInventory.AddInventoryCurrency(slot.item.GetItemBuyPrice());
            }
        }

        private void DonationCard(InventorySlot slot, int slotIndex) 
        {
            if(playerInventory.RemoveItem(slot.item, 1))
            {
                shopInventory.RemoveItem(slot.item, 1);
                playerInventory.AddPublicStanding(slot.item.GetItemStandingValue());
            }
        }
    }
}
