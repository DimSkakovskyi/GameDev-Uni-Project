using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoting : MonoBehaviour
{
    public Transform FirePoint;
    public FlyingChasingEnemy enemy;
    public GameObject bulletPrefab;

    private Transform playerTransform;

    public float fireCooldown = 1f;
    private float fireCooldownTimer = 0f;

    private Animator animator;


    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerTransform = playerGO.transform;
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (enemy != null && enemy.isChasing && playerTransform != null)
        {
            if (fireCooldownTimer <= 0f)
            {
                Shoot();
                SoundEffectManager.Play("Bat");
                fireCooldownTimer = fireCooldown;
            }
            else
            {
                fireCooldownTimer -= Time.deltaTime;
            }
        }
        else
        {
            fireCooldownTimer = 0f;
        }
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, FirePoint.position, Quaternion.identity);

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null && playerTransform != null)
        {
            bullet.SetDirection(playerTransform.position);

            Collider2D ownerCol = GetComponent<Collider2D>();
            bullet.SetOwner(ownerCol);
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }
}

