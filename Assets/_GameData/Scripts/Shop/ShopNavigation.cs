using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TSGameDev.UI
{
    public class ShopNavigation : MonoBehaviour
    {
        [Tooltip("The button used to either enable the next or previous section of the UI")]
        [SerializeField] Button nextButton, previousButton;

        [Tooltip("Array of all the sections that can be cycled through")]
        [SerializeField] GameObject[] sections;

        private int _CurrentSection;

        private void Awake()
        {
            _CurrentSection = 0;
            SetSection();
        }

        public void SetSection()
        {
            foreach(GameObject obj in sections)
                obj.SetActive(false);

            sections[_CurrentSection].SetActive(true);
        }

        public void CycleSectionNext()
        {
            _CurrentSection++;
            if (_CurrentSection >= sections.Length - 1)
            {
                _CurrentSection = sections.Length - 1;
                nextButton.interactable = false;
            }

            previousButton.interactable = true;
            SetSection();
        }

        public void CycleSectionPrevious()
        {
            _CurrentSection--;
            if (_CurrentSection <= 0)
            {
                _CurrentSection = 0;
                previousButton.interactable = false;
            }
            nextButton.interactable = true;
            SetSection();
        }
    }
}
