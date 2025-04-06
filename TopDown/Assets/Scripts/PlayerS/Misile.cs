using System.Collections;
using Enemies;
using UnityEngine;

public class Misile : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed;
    public float timeToLive;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        StartCoroutine(DestroyMisile());
    }
    
    private void FixedUpdate()
    {
        _rb.linearVelocity = transform.up * speed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out Player player)) return;
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(4);
        }
    }
    
    private IEnumerator DestroyMisile()
    {
        yield return new WaitForSeconds(timeToLive);
        gameObject.SetActive(false);
    }
}
