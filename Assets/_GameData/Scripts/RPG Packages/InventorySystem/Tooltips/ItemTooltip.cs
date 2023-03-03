using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace TSGameDev.Inventories.ToolTips
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        #region Serialized Variables
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI title;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected Image itemIcon;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI description;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI itemType;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI itemTier;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI minPriceBronze;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI maxPriceBronze;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI minPriceSilver;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI maxPriceSilver;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI minPriceGold;
        [TabGroup("tab1", "Generic Info")]
        [SerializeField] protected TextMeshProUGUI maxPriceGold;

        #endregion

        #region Public Functions

        /// <summary>
        /// The set up function for the tooltip, called by the spawner to correctly assign the items data.
        /// </summary>
        /// <param name="item">The hovered item in a ui</param>
        public virtual void Setup(InventoryItem item)
        {
            GeneralSetup(item);
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Function that performs the functionlaity for general setup correctly assigning information that is present in most or all items like the name and description.
        /// </summary>
        /// <param name="item">The item hovered in a UI container</param>
        private void GeneralSetup(InventoryItem item)
        {
            title.text = item.GetDisplayName();

            description.text = item.GetDescription();

            itemIcon.sprite = item.GetIcon();

            ItemTier Tier = item.GetTier();
            itemTier.text = $"{Enum.GetName(typeof(ItemTier), Tier)} Class Item";

            CurrencySet MinPrice = item.GetItemSellPrice();
            minPriceBronze.text = MinPrice.bronze.ToString();
            minPriceSilver.text = MinPrice.silver.ToString();
            minPriceGold.text = MinPrice.gold.ToString();

            CurrencySet MaxPrice = item.GetItemBuyPrice();
            maxPriceBronze.text = MaxPrice.bronze.ToString();
            maxPriceSilver.text = MaxPrice.silver.ToString();
            maxPriceGold.text = MaxPrice.gold.ToString();
        }

        #endregion
    }
}
