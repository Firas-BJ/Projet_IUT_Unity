using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 50f;
    public Rigidbody2D rb;
    public int damage = 10;
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnnemyPatrol enemy = hitInfo.GetComponent<EnnemyPatrol>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Debug.Log("detruit");
    }
    
}
