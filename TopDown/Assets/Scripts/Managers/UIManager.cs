 using System;
 using Enemies;
 using TMPro;
 using UnityEngine;
 using UnityEngine.Serialization;
 using UnityEngine.UI;

 namespace Managers
 {
  public class UIManager : MonoBehaviour
  {
      public Canvas gameOverCanvas;
      public TextMeshProUGUI coinText;
      public TextMeshProUGUI waveText;
      public TextMeshProUGUI scoreText;
      private int _score = 0;
      
      [SerializeField] private Slider healthSlider;
      [SerializeField] private Slider shieldSlider;
      [SerializeField] private TextMeshProUGUI overScoreText;
      [SerializeField] private TextMeshProUGUI overWaveText;


      private void OnEnable()
      {
          Player.HealthChange += UpdateHealth;
          Player.ShieldChange += UpdateShield;
          MoneyManager.MoneyCountChanged += UpdateMoney;
          EnemySpawner.WaveFinished += UpdateWave;
          Enemy.DeadEvent += UpdateScore;
          Upgrades.maxChange += SetBars;
      }

      private void OnDisable()
      {
          Player.HealthChange -= UpdateHealth;
          Player.ShieldChange -= UpdateShield;
          MoneyManager.MoneyCountChanged -= UpdateMoney;
          EnemySpawner.WaveFinished -= UpdateWave;
          Enemy.DeadEvent -= UpdateScore;
      }

      private void Start()
      {
          healthSlider.maxValue = 10;
          healthSlider.value = 10;
          shieldSlider.maxValue = 10;
          shieldSlider.value = 10;
      }

      private void SetBars(bool isHealth, int newMax)
      {
          if (isHealth)
          {
              healthSlider.maxValue = newMax;
          }
          else
          {
              shieldSlider.maxValue = newMax;
          }
      }
      
      private void UpdateHealth(int health)
      {
          healthSlider.value = health;
          if (health <= 0)
          {
              OnGameOver();
          }
      }

      private void UpdateShield(int shield)
      {
          shieldSlider.value = shield;
      }

      private void OnGameOver()
      {
          gameOverCanvas.enabled = true;
      }

      private void UpdateMoney(int money)
      {
          coinText.text = "$" + money;
      }

      private void UpdateWave(int wave)
      {
          waveText.text = "Current wave: " + wave;
          overWaveText.text = wave.ToString();
      }

      private void UpdateScore()
      {
          _score++;
          scoreText.text = "Score: " + _score;
          overScoreText.text = _score.ToString();
      }
  }
 }
