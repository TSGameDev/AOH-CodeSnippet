using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TSGameDev.Inventories;
using TSGameDev.Inventories.ToolTips;
using Sirenix.OdinInspector;

public class ReagentItemTooltip : ItemTooltip
{
    #region Serialized Variables

    [TabGroup("tab1", "Reagent Info")]
    [SerializeField] TextMeshProUGUI knownEffects;
    [TabGroup("tab1", "Reagent Info")]
    [SerializeField] TextMeshProUGUI knownProcesses;

    #endregion

    #region Public functions

    public override void Setup(InventoryItem item)
    {
        base.Setup(item);

        ReagentItem reagentItem = item as ReagentItem;
        ReagentSetup(reagentItem);
    }

    #endregion

    #region Private functions

    private void ReagentSetup(ReagentItem reagentItem)
    {
        if (reagentItem == null)
            return;

        itemType.text = "Reagent Ingredient";

        knownEffects.text = ReagentEffectSetup(reagentItem);
        knownProcesses.text = ReagentProcessingSetup(reagentItem);
    }

    private string ReagentEffectSetup(ReagentItem reagentItem)
    {
        string knownEffectBuilder = $"Known Effects: {Environment.NewLine} ";
        List<IngredientEffects> itemEffects = reagentItem.GetReagentEffects();

        foreach (IngredientEffects ingredientEffect in itemEffects)
        {
            string keyEffectName = Enum.GetName(typeof(Effects), ingredientEffect.effect);
            string EffectTier = ingredientEffect.effectTier.ToString();

            knownEffectBuilder += $"{keyEffectName} {EffectTier} {Environment.NewLine}";
        }
        return knownEffectBuilder;
    }

    private string ReagentProcessingSetup(ReagentItem reagentItem)
    {
        string newLine = Environment.NewLine;
        string knownProcessBuilder;

        knownProcessBuilder = $"Known Processes: {newLine}" +
            $"{MortarProcessingSetup(reagentItem)}" +
            $"{BlenderProcessingSetup(reagentItem)}" +
            $"{ChoppingProcessingSetup(reagentItem)}" +
            $"{BunsenProcessingSetup(reagentItem)}";


        return knownProcessBuilder;
    }

    private string MortarProcessingSetup(ReagentItem reagentItem)
    {
        string newLine = Environment.NewLine;
        string stringBuilder = "";
        InventoryItem MortarResult = reagentItem.GetMortarPrimaryResult();
        InventoryItem MortarSecResult = reagentItem.GetMortarSecondaryResult();
        if (MortarResult != null && MortarSecResult == null)
        {
            stringBuilder += $"Mortar: {MortarResult.GetDisplayName()} {newLine}";
        }
        else if(MortarResult != null && MortarSecResult != null)
        {
            stringBuilder += $"Mortar: {MortarResult.GetDisplayName()}, {MortarSecResult.GetDisplayName()}{newLine}";
        }

        return stringBuilder;
    }

    private string BlenderProcessingSetup(ReagentItem reagentItem)
    {
        string newLine = Environment.NewLine;
        string stringBuilder = "";
        InventoryItem BlenderResult = reagentItem.GetBlendingResult();
        if (BlenderResult != null)
        {
            stringBuilder += $"Blender: {BlenderResult.GetDisplayName()}{newLine}";
        }
        return stringBuilder;
    }
    

    private string ChoppingProcessingSetup(ReagentItem reagentItem)
    {
        string newLine = Environment.NewLine;
        string stringBuilder = "";
        InventoryItem ChoppingResult = reagentItem.GetChoppingPrimaryResult();
        InventoryItem ChoppingSecResult = reagentItem.GetChoppingSecondaryResult();
        if (ChoppingResult != null && ChoppingSecResult == null)
        {
            stringBuilder += $"Juicer: {ChoppingResult.GetDisplayName()}{newLine}";
        }
        else if(ChoppingResult != null && ChoppingSecResult != null)
        {
            stringBuilder += $"Juicer: {ChoppingResult.GetDisplayName()}, {ChoppingSecResult.GetDisplayName()}{newLine}";
        }
        return stringBuilder;
    }

    private string BunsenProcessingSetup(ReagentItem reagentItem)
    {
        string stringBuilder = "";
        List<BunsenBurnerProcess> BunsenResult = reagentItem.GetBunsenBurnerProcesses();
        if (BunsenResult != null)
        {
            stringBuilder += $"Bunsen: ";
            foreach (BunsenBurnerProcess process in BunsenResult)
            {
                stringBuilder += $"{process.minTemp} - {process.maxTemp} = {process.result.GetDisplayName()}, ";
            }
        }
        return stringBuilder;
    }

    #endregion
}
