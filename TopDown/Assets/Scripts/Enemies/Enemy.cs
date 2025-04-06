using System;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public int health;
        public float speed;
        private Transform _player;
        private Rigidbody2D _rb;
        
        public delegate void OnDeath();
        public static event OnDeath DeadEvent;
    
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _player = GameObject.Find("Player").transform;
        }

        private void OnEnable()
        {
            health = 4;
        }

        private void FixedUpdate()
        {
            var direction = (_player.position - transform.position).normalized;
            transform.rotation.SetLookRotation(_player.position-transform.position);
            _rb.linearVelocity = direction * speed;
        
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            gameObject.SetActive(false);
            PointsManager.Instance.AddPoints();
            DeadEvent?.Invoke();
        
            var probability = Random.Range(0, 10);
            if (probability >= 3)
            {
                MoneyManager.Instance.SpawnMoney(transform);
            }
        }
    
    }
}
