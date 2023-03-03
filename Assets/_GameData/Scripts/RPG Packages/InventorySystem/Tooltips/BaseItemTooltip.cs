using System.Collections;
using System.Collections.Generic;
using TSGameDev.Inventories;
using TSGameDev.Inventories.ToolTips;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using System;

public class BaseItemTooltip : ItemTooltip
{
    #region Serialized Variables

    [TabGroup("tab1", "Base Info")]
    [SerializeField] TextMeshProUGUI positiveAlchemicalBonusesTxt;
    [TabGroup("tab1", "Base Info")]
    [SerializeField] TextMeshProUGUI negativeAlchemicalBonusesTxt;
    [TabGroup("tab1", "Base Info")]
    [SerializeField] TextMeshProUGUI blockagesTxt;
    [TabGroup("tab1", "Base Info")]
    [SerializeField] TextMeshProUGUI imbueTxt;

    #endregion

    #region Private variables

    readonly string newline = $"{Environment.NewLine}";

    #endregion

    #region Public functions

    public override void Setup(InventoryItem item)
    {
        base.Setup(item);

        BaseItem baseItem = item as BaseItem;
        BaseSetup(baseItem);
    }

    #endregion

    #region Private Functions

    private void BaseSetup (BaseItem baseItem)
    {
        if (baseItem == null)
            return;

        itemType.text = "Base Ingredient";

        positiveAlchemicalBonusesTxt.text = PositiveBonusesToolTip(baseItem);
        negativeAlchemicalBonusesTxt.text = NegativeBonusesTooltip(baseItem);
        blockagesTxt.text = BloackagesToolTip(baseItem);
        imbueTxt.text = ImbueProcessingSetup(baseItem);
    }

    private string ImbueProcessingSetup(BaseItem baseItem)
    {
        string stringBuilder = "";
        ImbueProcess ImbueResult = baseItem.GetImbueResult();

        if (ImbueResult.baseItem == null || ImbueResult.reagentItem == null)
            return "";

        stringBuilder += $"{ImbueResult.baseItem.GetDisplayName()} + {ImbueResult.reagentItem.GetDisplayName()} = { baseItem.GetDisplayName() }";
        return stringBuilder;
    }

    private string PositiveBonusesToolTip(BaseItem baseItem)
    {
        string builderString = $"Positive Bonuses: {newline}";
        List<BaseAlchemicalEffects> positiveBonuses = baseItem.GetPositiveAlchemicalEffectBonuses();

        if (positiveBonuses == null)
            return builderString;

        foreach(BaseAlchemicalEffects bonus in positiveBonuses)
        {
            string effectName = Enum.GetName(typeof(Effects), bonus.effect);
            int effectPercentileBonus = bonus.effectPercentileBonus;

            builderString += $"{effectName} - {effectPercentileBonus}{newline}";
        }

        return builderString;
        
    }

    private string NegativeBonusesTooltip(BaseItem baseItem)
    {
        string builderString = $"Negative Bonuses: {newline}";
        List<BaseAlchemicalEffects> negativeBonuses = baseItem.GetNegativeAlchemicalEffectBonuses();

        if (negativeBonuses == null)
            return builderString;

        foreach (BaseAlchemicalEffects bonus in negativeBonuses)
        {
            string effectName = Enum.GetName(typeof(Effects), bonus.effect);
            int effectPercentileBonus = bonus.effectPercentileBonus;

            builderString += $"{effectName} - {effectPercentileBonus}{newline}";
        }
        return builderString;
    }

    private string BloackagesToolTip(BaseItem baseItem)
    {
        string builderString = $"Blockages: {newline}";
        List<BaseAlchemicalBlockages> blockages = baseItem.GetAlchemicalEffectBlockages();

        if (blockages == null)
            return builderString;

        foreach (BaseAlchemicalBlockages blockage in blockages)
        {
            string effectName = Enum.GetName(typeof(Effects), blockage.effect);
            int effectTier = blockage.effectTier;

            builderString += $"{effectName} - {effectTier}{newline}";
        }
        return builderString;
    }

    #endregion
}
