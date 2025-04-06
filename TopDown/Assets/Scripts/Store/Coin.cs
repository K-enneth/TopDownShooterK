using Managers;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out Player player)) return;
        MoneyManager.Instance.AddCoin();
        gameObject.SetActive(false);
    }
}
