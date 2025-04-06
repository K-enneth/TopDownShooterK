using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private GameObject shopCanvas;
        [SerializeField] private List<GameObject> shopItems;
        [SerializeField] private GameObject itemHolder;
        private void OnEnable()
        {
            EnemySpawner.WaveFinished += OpenShop;
            EnemySpawner.WaveFinished += CheckAvailability;
            MoneyManager.MoneyCountChanged += CheckAvailability;
        }

        private void OnDisable()
        {
            EnemySpawner.WaveFinished -= OpenShop;
            EnemySpawner.WaveFinished -= CheckAvailability;
            MoneyManager.MoneyCountChanged -= CheckAvailability;

        }

        private void Start()
        {
            for (var indexItem = 0; indexItem < itemHolder.transform.childCount; indexItem++)
            {
                shopItems.Add(itemHolder.transform.GetChild(indexItem).gameObject);
            }
        }

        private void OpenShop(int waveIndex)
        {
            shopCanvas.GetComponent<Canvas>().enabled = true;
        }

        public void CloseShop()
        {
            shopCanvas.GetComponent<Canvas>().enabled = false;
        }

        private void CheckAvailability(int waveIndex)
        {
            for (var indexButton = 0; indexButton < shopItems.Count; indexButton++)
            {
                var currentUpgrade = shopItems[indexButton].GetComponent<Upgrades>();
                if (currentUpgrade.cost > MoneyManager.Instance.money)
                {
                    shopItems[indexButton].GetComponent<UnityEngine.UI.Button>().interactable = false;
                }
                else
                {
                    shopItems[indexButton].GetComponent<UnityEngine.UI.Button>().interactable = true;
                }
            }
        }
        
        
    }
}
