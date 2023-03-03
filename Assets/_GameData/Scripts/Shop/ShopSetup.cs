using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Core.Shop
{
    public class ShopSetup : MonoBehaviour
    {
        [SerializeField] List<Shop> shopList;

        private void Awake()
        {
            foreach(Shop shop in shopList)
            {
                shop.Initialisation();
            }
        }
    }
}
