using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float speed;
    public bool isPlayerBullet;

    void Update()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isPlayerBullet) return;

            else
            {
                other.GetComponent<PlayerHandler>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        
        else if (other.GetComponent<EnemyHandler>() != null)
        {
            other.GetComponent<EnemyHandler>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
