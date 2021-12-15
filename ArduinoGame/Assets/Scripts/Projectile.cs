using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject ParticlesPrefab;
    public int Time = 2;

    public bool IsHolderEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" || collision.tag == "Collectible")
        {
            Destroy(gameObject);
        }
        if (!IsHolderEnemy)
        {
            if (collision.tag == "Enemy")
            {
                Destroy(gameObject, 0.01f);
            }
            if (collision.tag == "Boss")
            {
                Destroy(gameObject, 0.01f);
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                Destroy(gameObject, 0.01f);
            }
        }
    }

    private void OnDestroy()
    {
        GameObject a_projectile = Instantiate(ParticlesPrefab, transform.position, transform.rotation);
    }
}
