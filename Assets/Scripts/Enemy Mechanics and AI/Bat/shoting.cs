using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoting : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform FirePoint;
    public FlyingChasingEnemy enemy;
    public GameObject bulletPrefab;
    public Transform playerTransform;

    public float fireCooldown = 1f; // ⏱ Інтервал між пострілами в секундах
    private float fireCooldownTimer = 0f;

    // Update is called once per frame
    void Update()
    {

        if (enemy != null && enemy.isChasing)
        {
            if (fireCooldownTimer <= 0f)
            {
                Shoot();
                fireCooldownTimer = fireCooldown;
            }
            else
            {
                fireCooldownTimer -= Time.deltaTime;
            }
        }
        else
        {
            // Якщо гравець вийшов з агро-зони, таймер скидається (не обов'язково)
            fireCooldownTimer = 0f;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, FirePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null && playerTransform != null)
        {
            bulletScript.SetDirection(playerTransform.position);
        }
    }
}
