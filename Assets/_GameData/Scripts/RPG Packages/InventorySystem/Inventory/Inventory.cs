using System;
using UnityEngine;
using TSGameDev.SavingSystem;
using System.Collections.Generic;

namespace TSGameDev.Inventories
{
    /// <summary>
    /// Provides storage for the player inventory. A configurable number of
    /// slots are available.
    ///
    /// This component should be placed on the GameObject tagged "Player".
    /// </summary>
    [CreateAssetMenu(menuName = ("TSGameDev/Inventory/Inventory"), fileName = "New Inventory", order = 1)]
    public class Inventory : ScriptableObject, ISaveable
    {
        #region Serialized Variables

        [Tooltip("The amount of slots within this inventory.")]
        [SerializeField] int inventorySize = 16;
        InventorySlot[] slots;
        [SerializeField] CurrencySet inventoryCurrency;
        [SerializeField] float publicStanding = 0f;

        #endregion

        #region Additional Data Sets

        //Struct defining what an InventorySlot is, allows for assinging an item reference to an amount of items.
        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        //Struct defining what an InventorySlotRecord is. Is the serialiable version of InventionSlot struct which takes an ItemID instead of Item allowing for the struct to be savable.
        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action OnInventoryUpdated;
        public event Action<CurrencySet> OnCurrencyUpdated;

        #endregion

        #region Public Function

        /// <summary>
        /// Getter for the invetory slots list. Used by shops to spawn cards within their UI's
        /// </summary>
        /// <returns>Array of inventorySlots</returns>
        public InventorySlot[] GetInventory() => slots;

        /// <summary>
        /// Getter for the inventories curreny.
        /// </summary>
        /// <returns>CurrencySet, a custom data structure with bronze, silver and gold amounts</returns>
        public CurrencySet GetInventoryCurrency() => inventoryCurrency;

        /// <summary>
        /// Getter for the inventories standing amount.
        /// </summary>
        /// <returns>float</returns>
        public float GetPublicStanding() => publicStanding;

        /// <summary>
        /// Function for adding an amount to the inventories standing value.
        /// </summary>
        /// <param name="amount">The float amount you wish to add to the standing</param>
        public void AddPublicStanding(float amount) => publicStanding += amount;

        /// <summary>
        /// Function for removing an amount from the inventories standing value.
        /// </summary>
        /// <param name="amount">the float amount you wish to remove from the standing</param>
        public void RemovePublicStanding(float amount) => publicStanding -= amount;

        /// <summary>
        /// Function to add Currency to this inventory
        /// </summary>
        /// <param name="amountToAdd">CurrencySet for the amount you wish to add to this inventory</param>
        public void AddInventoryCurrency(CurrencySet amountToAdd)
        {
            inventoryCurrency.bronze += amountToAdd.bronze;
            if(inventoryCurrency.bronze >= 100)
            {
                inventoryCurrency.bronze -= 100;
                inventoryCurrency.silver++;
            }

            inventoryCurrency.silver += amountToAdd.silver;
            if(inventoryCurrency.silver >= 100)
            {
                inventoryCurrency.silver -= 100;
                inventoryCurrency.gold++;
            }

            inventoryCurrency.gold+= amountToAdd.gold;
            if(OnCurrencyUpdated != null)
                OnCurrencyUpdated.Invoke(inventoryCurrency);
        }

        /// <summary>
        /// Function to remove currency from this inventory
        /// </summary>
        /// <param name="amountToRemove">the CurrencySet for the amount you wish to remove</param>
        /// <returns>Bool for if the currency amount could be removed</returns>
        public bool RemoveInventoryCurrency(CurrencySet amountToRemove)
        {
            bool ConvertedBronze = false;
            bool ConvertedSilver = false;

            if(inventoryCurrency.bronze < amountToRemove.bronze)
            {
                inventoryCurrency.bronze += 100;
                inventoryCurrency.silver--;
                ConvertedBronze = true;
            }

            if (inventoryCurrency.silver < amountToRemove.silver)
            {
                inventoryCurrency.silver += 100;
                inventoryCurrency.gold--;
                ConvertedSilver = true;
            }

            if (inventoryCurrency.gold < amountToRemove.gold)
            {
                if(ConvertedBronze)
                {
                    inventoryCurrency.bronze -= 100;
                    inventoryCurrency.silver++;
                }

                if(ConvertedSilver)
                {
                    inventoryCurrency.silver -= 100;
                    inventoryCurrency.gold++;
                }
                return false;
            }

            inventoryCurrency.bronze -= amountToRemove.bronze;
            inventoryCurrency.silver -= amountToRemove.silver;
            inventoryCurrency.gold -= amountToRemove.gold;
            if (OnCurrencyUpdated != null)
                OnCurrencyUpdated.Invoke(inventoryCurrency);
            return true;
        }

        /// <summary>
        /// Could this item fit anywhere in the inventory?
        /// </summary>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        /// <summary>
        /// How many slots are in the inventory?
        /// </summary>
        public int GetSize()
        {
            return slots.Length;
        }

        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="number">The number to add.</param>
        /// <returns>Whether or not the item could be added.</returns>
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].number += number;
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public int GetNumberInSlot(int slot)
        {
            return slots[slot].number;
        }

        /// <summary>
        /// Remove a number of items from the given slot. Will never remove more
        /// that there are.
        /// </summary>
        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0)
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is already
        /// a stack of this type, it will add to the existing stack. Otherwise,
        /// it will be added to the first empty slot.
        /// </summary>
        /// <param name="slot">The slot to attempt to add to.</param>
        /// <param name="item">The item type to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
            return true;
        }

        /// <summary>
        /// Loops through the inventory for mathching item elements and attempts to remove the amount from that element. Will return fail if a single group can't remove the specified amount.
        /// AkA doesn't support removing items from multiple elements in the inventory so its best to keep items in single groups.
        /// </summary>
        /// <param name="item">The item you wish to remove</param>
        /// <param name="number">The amount of the item you wish to remove.</param>
        /// <returns>Returns bool for a successful or failed attempt. True is successul, false is a failure. Can fail if first element doesn't have enough items.</returns>
        public bool RemoveItem(InventoryItem item, int number)
        {
            for(int i = 0; i <= slots.Length - 1; i++)
            {
                if (slots[i].item == item)
                {
                    if (slots[i].number >= number)
                    {
                        RemoveFromSlot(i, number);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Function to remove all items from the inventory.
        /// </summary>
        public void ClearInventory()
        {
            for(int i = 0; i <= slots.Length - 1; i++)
            {
                slots[i].item = null;
                slots[i].number = 0;
            }

            if (OnInventoryUpdated != null)
                OnInventoryUpdated.Invoke();
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Function that is called when this script is enabled, creates a new blank inventory
        /// </summary>
        private void OnEnable()
        {
            slots = new InventorySlot[inventorySize];
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }
    
        /// <summary>
        /// Function implimented by ISavable that defines how this script should save its data
        /// </summary>
        /// <returns>
        /// An object that contains all the serializable data for saving
        /// </returns>
        object ISaveable.CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotStrings[i].itemID = slots[i].item.GetItemID();
                    slotStrings[i].number = slots[i].number;
                }
            }
            return slotStrings;
        }

        /// <summary>
        /// Function implimented by ISavable that defines how this script should loud passed in data
        /// </summary>
        /// <param name="state">
        /// Object that contains all the save data, cast to array of InventorySlotRecord.
        /// </param>
        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
                slots[i].number = slotStrings[i].number;
            }
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated();
            }
        }

        #endregion

    }
}