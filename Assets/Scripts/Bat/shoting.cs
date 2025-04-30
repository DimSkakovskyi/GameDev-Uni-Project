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
        GameObject bulletGO = Instantiate(bulletPrefab, FirePoint.position, Quaternion.identity);

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (player != null)
            {
                bullet.SetDirection(player.position);

                // Передаємо колайдер ворога
                Collider2D ownerCol = GetComponent<Collider2D>();
                bullet.SetOwner(ownerCol);
            }
        }
    }
}
