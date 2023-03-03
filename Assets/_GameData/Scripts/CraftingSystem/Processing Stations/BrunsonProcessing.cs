using System.Collections.Generic;
using TMPro;
using TSGameDev.UI.Inventories.Crafting;
using UnityEngine;
using UnityEngine.UI;

namespace TSGameDev.Inventories.Crafting
{
    public class BrunsonProcessing : MonoBehaviour
    {
        [SerializeField] CraftingSlotUI craftSlot;
        [SerializeField] CraftingResultSlotUI resultSlot;
        [SerializeField] Slider tempControl;
        [SerializeField] TextMeshProUGUI tempText;

        public void ProcessingIngrediant()
        {
            int Currenttemp = (int)tempControl.value;
            ReagentItem reagentItem = craftSlot.GetItem() as ReagentItem;
            List<BunsenBurnerProcess> processes = reagentItem.GetBunsenBurnerProcesses();

            foreach(BunsenBurnerProcess process in processes)
            {
                if(Currenttemp > process.minTemp && Currenttemp < process.maxTemp)
                {
                    craftSlot.RemoveItems(1);
                    resultSlot.AddItem(process.result, Random.Range(1, 5));
                }
            }
        }

        public void TempTxt() => tempText.text = $"{(int)tempControl.value}C";
    }
}

