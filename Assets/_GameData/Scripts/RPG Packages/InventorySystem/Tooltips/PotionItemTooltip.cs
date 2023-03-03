using System.Collections.Generic;
using TSGameDev.Inventories;
using TSGameDev.Inventories.ToolTips;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using System;

public class PotionItemTooltip : ItemTooltip
{
    #region Serialized Variables

    [TabGroup("tab1", "Base Info")]
    [SerializeField] TextMeshProUGUI positiveAlchemicalBonusesTxt;
    [TabGroup("tab1", "Base Info")]
    [SerializeField] TextMeshProUGUI negativeAlchemicalBonusesTxt;
    [TabGroup("tab1", "Base Info")]
    [SerializeField] TextMeshProUGUI blockagesTxt;

    [TabGroup("tab1", "Potion Info")]
    [SerializeField] TextMeshProUGUI potionActiveEffect;

    #endregion

    #region Private variables

    readonly string newline = $"{Environment.NewLine}";

    #endregion

    #region Public functions

    public override void Setup(InventoryItem item)
    {
        base.Setup(item);

        PotionItem potionItem = item as PotionItem;
        PotionSetup(potionItem);
    }

    #endregion

    #region Private Functions

    private void PotionSetup (PotionItem potionItem)
    {
        if (potionItem == null)
            return;

        itemType.text = "Potion Ingredient";

        positiveAlchemicalBonusesTxt.text = PositiveBonusesToolTip(potionItem);
        negativeAlchemicalBonusesTxt.text = NegativeBonusesTooltip(potionItem);
        blockagesTxt.text = BloackagesToolTip(potionItem);

        potionActiveEffect.text = potionItem.GetPotionActiveEffect();

    }

    private string PositiveBonusesToolTip(PotionItem baseItem)
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

    private string NegativeBonusesTooltip(PotionItem baseItem)
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

    private string BloackagesToolTip(PotionItem baseItem)
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
