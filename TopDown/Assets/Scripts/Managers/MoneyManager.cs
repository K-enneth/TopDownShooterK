using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        public static MoneyManager Instance;
        public PoolingSystem coinPool;
        public int money;
        
        public delegate void OnMoneyCount(int moneyCount);
        public static event OnMoneyCount MoneyCountChanged;
        private void Awake()
        {
            if(Instance == null)Instance = this;
            else Destroy(gameObject);
        }

        public void SpawnMoney(Transform spawnPosition)
        {
            coinPool.AskForObject(spawnPosition);
        }

        public void AddCoin()
        {
            money++;
            MoneyCountChanged?.Invoke(money);
        }

        public void WasteCoin(int cost)
        {
            money -= cost;
            MoneyCountChanged?.Invoke(money);
        }
    }
}
