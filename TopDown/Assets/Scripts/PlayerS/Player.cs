using System;
using System.Collections;
using Enemies;
using Managers;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera _camera;

    [Header("Movement")]
    public float speed;
    private Rigidbody2D _rb;
    private Vector3 _mousePos;
    private Vector2 _direction;
    private bool _canMove = true;

    [Header("Bullets")]
    [SerializeField] private PoolingSystem bulletPool;
    public float timeBetweenBullets;
    private bool _canShoot = true;
    
    [Header("Misiles")]
    public int maxMisiles;
    public int currentMisiles;
    [SerializeField] private PoolingSystem misilePool;
    public float timeBetweenMisiles;
    private bool _misileShoot = true;
    

    [Header("Health and Shield")]
    public int maxHealth;
    public int health;
    public int maxShield;
    public int shield;
    public float invincibilityTime;
    public float shieldRegenTime;
    public float shieldRegenRate;
    private Coroutine _corRegen;
    private bool _canTakeDamage = true;
    
    public delegate void OnHealthChange(int newHealth);
    public static event OnHealthChange HealthChange;
    public delegate void OnShieldChange(int newShield);
    public static event OnShieldChange ShieldChange;


    private void OnEnable()
    {
        EnemySpawner.WaveFinished += DisableInput;
    }

    private void OnDisable()
    {
        EnemySpawner.WaveFinished -= DisableInput;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }
    void Update()
    {
        if(!_canMove) return;
        _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        FollowMouse();
        
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
        _direction.Normalize();

        if (Input.GetMouseButton(0) && (_canShoot))
        {
            StartCoroutine(ShootBullet());
        }

        if (Input.GetMouseButton(1) && (_misileShoot) && (currentMisiles > 0) )
        {
            StartCoroutine(ShootMisile());
        }
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }

    void FollowMouse()
    {
        var angle = Mathf.Atan2(_mousePos.y - transform.position.y, _mousePos.x - transform.position.x);
        var angleFixed = (180 / Mathf.PI) * angle - 90;
        
        transform.rotation = Quaternion.Euler(0,0,angleFixed);
    }

    private IEnumerator ShootBullet()
    {
        var bullet = bulletPool.AskForObject(transform);
        _canShoot = false;
        yield return new WaitForSeconds(timeBetweenBullets);
        _canShoot = true;
    }

    private IEnumerator ShootMisile()
    {
        misilePool.AskForObject(transform);
        _misileShoot = false;
        currentMisiles--;
        yield return new WaitForSeconds(timeBetweenMisiles);
        _misileShoot = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!_canTakeDamage || !other.gameObject.TryGetComponent(out Enemy enemy))return;
        
        if (shield > 0)
        {
            shield -= 1;
            ShieldChange?.Invoke(shield);
        }
        else
        {
            health -= 1;
            HealthChange?.Invoke(health);
            
            if(health <= 0)
            {
               DisableInput(1);
            }
        }
        StartCoroutine(InvincibilityCooldown());
        
        if (_corRegen != null)
        {
            StopCoroutine(_corRegen);
        }
        
        _corRegen = StartCoroutine(ShieldRegen());

    }

    private IEnumerator InvincibilityCooldown()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(invincibilityTime);
        _canTakeDamage = true;
    }

    private IEnumerator ShieldRegen()
    {
        yield return new WaitForSeconds(shieldRegenTime);
        while (shield < maxShield && _canMove)
        {
            shield += 1;    
            ShieldChange?.Invoke(shield);
            yield return new WaitForSeconds(shieldRegenRate);
        }
    }

    public void RefillHealth()
    {
        health = maxHealth;
        HealthChange?.Invoke(health);
    }

    private void DisableInput(int wave)
    {
        _canMove = false;
        _rb.linearVelocity = Vector2.zero;
    }

    public void EnableInput()
    {
        _canMove = true;
        _corRegen = StartCoroutine(ShieldRegen());
    }
    
}
