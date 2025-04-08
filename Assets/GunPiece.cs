using UnityEngine;

public class GunPiece : ShipPiece
{
    public int shotsPerSecond = 2;
    public float bulletDamage = 10;
    public float bulletSpeed = 25f;
    private float timeSinceLastShot;
    public Transform spawnBulletPoint;
    public GameObject bulletPrefab;

    public void FireButtonPressed()
    {
        float delay = 1.0f / shotsPerSecond;
        
        if (delay <= timeSinceLastShot)
        {
            timeSinceLastShot = 0.0f;
            Fire();
        }
    }

    public void Fire()
    {
        Bullet newBullet = Instantiate(bulletPrefab, spawnBulletPoint.position, spawnBulletPoint.rotation).GetComponent<Bullet>();
        newBullet.damage = bulletDamage;
        newBullet.speed = bulletSpeed;
        newBullet.isPlayerBullet = true;
    }

    private void Update()
    {
        timeSinceLastShot+= Time.deltaTime;
    }
}
