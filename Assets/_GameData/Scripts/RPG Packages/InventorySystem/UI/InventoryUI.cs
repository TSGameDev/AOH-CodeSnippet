using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TSGameDev.Inventories;

namespace TSGameDev.UI.Inventories
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        #region Serialized Variables

        // Prefab of Item slots that represent positions in a inventory
        [SerializeField] InventorySlotUI InventoryItemPrefab;

        // reference to the player inventory
        [SerializeField] Inventory Displayinventory;
        public Inventory GetInventory() => Displayinventory;
        public void SetInventory(Inventory newInventory) => Displayinventory = newInventory;

        #endregion

        #region Life Cycle Functions

        private void Awake() 
        {
            Displayinventory.OnInventoryUpdated += this.Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        private void OnDestroy()
        {
            Displayinventory.OnInventoryUpdated -= this.Redraw;
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Function reposible for updating the inventory UI whenever there is a change. Simply destorys all the UI and re-instantiates the gameobjects calling Setup on each one
        /// This makes the UI update as the changes to the inventory have already happened meaning the position of an item has changed or a new item has already been added therefor will be acounted for in the UI.
        /// </summary>
        public void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < Displayinventory.GetSize(); i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.Setup(Displayinventory, i);
            }
        }

        #endregion

    }
}