using System;
using System.Collections.Generic;
using UnityEngine;
using TSGameDev.Inventories.Pickups;
using Sirenix.OdinInspector;

namespace TSGameDev.Inventories
{
    [Serializable]
    public struct CurrencySet
    {
        public CurrencySet(int newBronze = 0, int newSilver = 0, int newGold = 0)
        {
            bronze = newBronze;
            silver = newSilver;
            gold = newGold;
        }

        public int bronze;
        public int silver;
        public int gold;
    }

    [Serializable]
    public enum Effects
    {
        Heal,
        Cure,
        Health,
        Regen,
        Poison,
        Harm,
        Stun,
        Slow
    }

    [Serializable]
    public enum ItemTier
    {
        Common,
        Uncommon,
        Rare,
        VeryRare,
        Epic,
        Exotic,
        Fabled,
        Divine,
        Tyrant,
        God,

    }

    /// <summary>
    /// A ScriptableObject that represents any item that can be put in an
    /// inventory.
    /// </summary>
    /// <remarks>
    /// In practice, you are likely to use a subclass such as `ActionItem` or
    /// `EquipableItem`.
    /// </remarks>
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Serialized Variables

        [TabGroup("Tab1", "General Information")]
        [HorizontalGroup("Tab1/General Information/Split", width: 0.21f)]
        [VerticalGroup("Tab1/General Information/Split/Left")]
        [Tooltip("The UI icon to represent this item in the inventory.")]
        [PreviewField(100, ObjectFieldAlignment.Left)]
        [HideLabel]
        [SerializeField] Sprite icon = null;

        [VerticalGroup("Tab1/General Information/Split/Left")]
        [Tooltip("Item Tier")]
        [LabelWidth(53)]
        [MinValue(1)]
        [MaxValue(10)]
        [SerializeField] ItemTier itemTier = ItemTier.Common;

        [VerticalGroup("Tab1/General Information/Split/Right")]
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [LabelWidth(48)]
        [SerializeField] string itemID = null;

        [VerticalGroup("Tab1/General Information/Split/Right")]
        [Tooltip("Item name to be displayed in UI.")]
        [LabelWidth(85)]
        [SerializeField] string displayName = null;

        [VerticalGroup("Tab1/General Information/Split/Right")]
        [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
        [LabelWidth(100)]
        [SerializeField] bool stackable = false;

        [VerticalGroup("Tab1/General Information/Split/Right")]
        [Tooltip("Item description to be displayed in UI.")]
        [SerializeField][TextArea] string description = null;

        [TabGroup("Tab1", "Pickup Information")]
        [Tooltip("The prefab that should be spawned when this item is dropped.")]
        [SerializeField] Pickup pickup = null;

        [TabGroup("Tab1", "Price & Shop Information")]
        [PropertyTooltip("Bool for if this item can be sold.")]
        [SerializeField] bool sellable = true;

        [TabGroup("Tab1", "Price & Shop Information")]
        [PropertyTooltip("The price the item sells for")]
        [SerializeField] CurrencySet itemSellPrice = new();

        [TabGroup("Tab1", "Price & Shop Information")]
        [PropertyTooltip("The price the item can be bought for from a shop")]
        [SerializeField] CurrencySet itemBuyPrice = new();

        [TabGroup("Tab1", "Price & Shop Information")]
        [PropertyTooltip("The Price the item can sol")]
        [SerializeField] float itemStandingValue;

        [TabGroup("Tab1", "Price & Shop Information")]
        [PropertyTooltip("The chance this item will spawn in shops as a percentage. If this is 5 the shop must roll a 5 or less for this item to spawn aka 5% chance")]
        [SerializeField] int shopChance = 10;

        [TabGroup("Tab1", "Price & Shop Information")]
        [PropertyTooltip("The max amount of this itme that will spawn in a show.")]
        [SerializeField] int shopMaxAmount = 1;

        #endregion

        #region Static Private Variables

        public static Dictionary<string, PotionItem> potionLookupCache;
        public static Dictionary<string, ReagentItem> reagentLookupCache;
        public static Dictionary<string, BaseItem> baseLookupCache;

        static readonly string potionResourcePath = "Potions";
        static readonly string reagentResourcePath = "Reagents";
        static readonly string baseResourcePath = "Bases";

        #endregion

        #region Public Functions

        /// <summary>
        /// Get the inventory item instance from its UUID.
        /// </summary>
        /// <param name="itemID">
        /// String UUID that persists between game instances.
        /// </param>
        /// <returns>
        /// Inventory item instance corresponding to the ID.
        /// </returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (potionLookupCache == null)
                CreateItemCaches();

            if (itemID == null || !potionLookupCache.ContainsKey(itemID)) return null;
            return potionLookupCache[itemID];
        }
        
        /// <summary>
        /// Static Function used to create the global item caches for the crafting system if the caches have not yet been created via GetFromID function
        /// </summary>
        public static void CreateItemCaches()
        {
            CreatePotionCache();
            CreateReagentCache();
            CreateBaseCache();
        }

        /// <summary>
        /// Spawn the pickup gameobject into the world.
        /// </summary>
        /// <param name="position">Where to spawn the pickup.</param>
        /// <param name="number">How many instances of the item does the pickup represent.</param>
        /// <returns>Reference to the pickup object spawned.</returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            if (this.pickup == null)
                return null;

            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        /// <summary>
        /// Function that returns the Icon of this item
        /// </summary>
        /// <returns>
        /// a icon Sprite of this item
        /// </returns>
        public Sprite GetIcon()
        {
            return icon;
        }

        /// <summary>
        /// Fucntion that returns the ID of this item
        /// </summary>
        /// <returns>
        /// the id string of this item
        /// </returns>
        public string GetItemID()
        {
            return itemID;
        }

        /// <summary>
        /// Function that returns if this item is stackable or not.
        /// </summary>
        /// <returns>
        /// a bool for if this item is stackable.
        /// </returns>
        public bool IsStackable()
        {
            return stackable;
        }
        
        /// <summary>
        /// function that returns the display name of this item.
        /// </summary>
        /// <returns>
        /// a name string of this item.
        /// </returns>
        public string GetDisplayName()
        {
            return displayName;
        }

        /// <summary>
        /// Function that returns the description of this item
        /// </summary>
        /// <returns>
        /// a description string of this item.
        /// </returns>
        public string GetDescription()
        {
            return description;
        }

        /// <summary>
        /// Function that returns if the item is sellable.
        /// </summary>
        /// <returns>
        /// Bool value for if the item is sellable.
        /// </returns>
        public bool GetSellable()
        {
            return sellable;
        }

        /// <summary>
        /// function that returns the min price of this item.
        /// </summary>
        /// <returns>
        /// a number int of the price of this item.
        /// </returns>
        public CurrencySet GetItemSellPrice()
        {
            return itemSellPrice;
        }

        /// <summary>
        /// function that returns the max price of this item.
        /// </summary>
        /// <returns>
        /// a number int of the price of this item.
        /// </returns>
        public CurrencySet GetItemBuyPrice()
        {
            return itemBuyPrice;
        }

        /// <summary>
        /// Getter function that returns the items standing value for when the item is donated.
        /// </summary>
        /// <returns>float</returns>
        public float GetItemStandingValue()
        {
            return itemStandingValue;
        }

        /// <summary>
        /// function that returns the tier of this item.
        /// </summary>
        /// <returns>
        /// a number int of the tier of this item.
        /// </returns>
        public ItemTier GetTier()
        {
            return itemTier;
        }

        /// <summary>
        /// Getter for the items shop chance. A percentage for this item to spawn it shops its assigned to.
        /// </summary>
        /// <returns>A doulbe repreenting percentage chance</returns>
        public int GetShopChance() => shopChance;

        /// <summary>
        /// Getter for the amount of this item that can appear in a shop
        /// </summary>
        /// <returns>int</returns>
        public int GetMaxShopAmount() => shopMaxAmount;

        #endregion

        #region Private Functions

        // Static function that creates the global cache for potion items.
        static void CreatePotionCache()
        {
            potionLookupCache = new();
            var itemList = Resources.LoadAll<PotionItem>(potionResourcePath);
            foreach (var item in itemList)
            {
                if (potionLookupCache.ContainsKey(item.itemID))
                {
                    Debug.LogError($"Duplicate Inventory Item ID's for Objects: {potionLookupCache[item.itemID]} and {item}");
                    continue;
                }
                potionLookupCache[item.itemID] = item;
            }
        }

        // Static function that creates the global cache for reagent items.
        static void CreateReagentCache()
        {
            reagentLookupCache = new();
            var itemList = Resources.LoadAll<ReagentItem>(reagentResourcePath);
            foreach(var item in itemList)
            {
                if(reagentLookupCache.ContainsKey(item.itemID))
                {
                    Debug.LogError($"Duplicate Inventory Item ID's for Objects: {potionLookupCache[item.itemID]} and {item}");
                    continue;
                }
                reagentLookupCache[item.itemID] = item;
            }
        }

        // Static function that creates the global cache for base items.
        static void CreateBaseCache()
        {
            baseLookupCache = new();
            var itemList = Resources.LoadAll<BaseItem>(baseResourcePath);
            foreach (var item in itemList)
            {
                if (baseLookupCache.ContainsKey(item.itemID))
                {
                    Debug.LogError($"Duplicate Inventory Item ID's for Objects: {baseLookupCache[item.itemID]} and {item}");
                    continue;
                }
                baseLookupCache[item.itemID] = item;
            }
        }

        /// <summary>
        /// Function that is called when variable changes and the script is Serialized by Unity. Checks if the ID is unique and if it needs to generate a new guild ID
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        //Function require by the interface but isn't used.
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }

        #endregion

    }

}
