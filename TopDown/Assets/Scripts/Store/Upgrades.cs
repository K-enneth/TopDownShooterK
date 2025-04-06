using System;
using Managers;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    private Player _player;
    public int cost;
    
    public delegate void OnMaxChange(bool isHealth, int value);
    public static event OnMaxChange maxChange;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void FillHealth()
    {
        _player.RefillHealth();
        Waste();
    }

    public void MaxHealth()
    {
        _player.maxHealth = _player.maxHealth += 5;
        maxChange?.Invoke(true, _player.maxHealth);
        Waste();
    }

    public void MaxShield()
    {
        _player.maxShield = _player.maxShield += 5;
        maxChange?.Invoke(false, _player.maxShield);
        Waste();
    }

    public void ReturnMisiles()
    {
        _player.currentMisiles = _player.maxMisiles;
        Waste();
    }

    public void MaxMisiles()
    {
        _player.maxMisiles = _player.maxMisiles++;
        Waste();
    }

    public void ShieldRegen()
    {
        _player.shieldRegenTime = _player.shieldRegenTime -= 0.2f;
        Waste();
    }

    private void Waste()
    {
        MoneyManager.Instance.WasteCoin(cost);
    }
}
