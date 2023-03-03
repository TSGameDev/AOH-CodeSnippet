using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace TSGameDev.Inventories
{
    [Serializable]
    public struct BaseAlchemicalEffects
    {
        public Effects effect;

        [MinValue(1)]
        [MaxValue(100)]
        public int effectPercentileBonus;

        public BaseAlchemicalEffects(Effects effect, int effectPercentileBonus = 1)
        {
            this.effect = effect;
            this.effectPercentileBonus = effectPercentileBonus;
        }
    }

    [Serializable]
    public struct BaseAlchemicalBlockages
    {
        public Effects effect;
        public int effectTier;

        public BaseAlchemicalBlockages(Effects effect, int effectTier)
        {
            this.effect = effect;
            this.effectTier = effectTier;
        }
    }

    [Serializable]
    public struct ImbueProcess
    {
        public ReagentItem reagentItem;
        public BaseItem baseItem;

        public ImbueProcess(BaseItem baseItem, ReagentItem reagentItem)
        {
            this.reagentItem = reagentItem;
            this.baseItem = baseItem;
        }
    }

    [CreateAssetMenu(menuName = ("TSGameDev/Inventory/New Base Item"))]
    public class BaseItem : InventoryItem
    {
        #region Serialsied Variables

        [TabGroup("Tab1", "Base Information")]
        [PropertyTooltip("The recipe for this base to be created for via the imbue crafting station")]
        [SerializeField] ImbueProcess imbueResult; 

        [TabGroup("Tab1", "Base Information")]
        [PropertyTooltip("A list of all the effect this ingredient provides to the potions it creates")]
        [SerializeField] List<BaseAlchemicalEffects> positiveAlchemicalBonuses;

        [TabGroup("Tab1", "Base Information")]
        [PropertyTooltip("A list of all the effect this ingredient provides to the potions it creates")]
        [SerializeField] List<BaseAlchemicalEffects> negativeAlchemicalBonuses;

        [TabGroup("Tab1", "Base Information")]
        [PropertyTooltip("A list of the effects that can't be applied to a potion using this base unless the effect has a higher tier than the tier shown.")]
        [SerializeField] List<BaseAlchemicalBlockages> alchemicalEffectBlockages;

        #endregion

        #region Public functions

        public ImbueProcess GetImbueResult() => imbueResult;

        public List<BaseAlchemicalEffects> GetPositiveAlchemicalEffectBonuses()
        {
            return positiveAlchemicalBonuses;
        }

        public List<BaseAlchemicalEffects> GetNegativeAlchemicalEffectBonuses()
        {
            return negativeAlchemicalBonuses;
        }

        public List<BaseAlchemicalBlockages> GetAlchemicalEffectBlockages()
        {
            return alchemicalEffectBlockages;
        }

        #endregion

    }
}
