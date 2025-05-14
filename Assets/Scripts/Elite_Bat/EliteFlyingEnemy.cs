using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteFlyingEnemy : FlyingChasingEnemy
{
    [Header("Orbiting Bullets Settings")]
    public GameObject bulletPrefab;
    public int bulletCount = 4;
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 90f;

    private GameObject[] orbitingBullets;
    private float orbitAngleOffset;

    protected override void Start()
    {
        base.Start();
        SpawnOrbitingBullets();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateOrbitingBullets();
    }

    private void SpawnOrbitingBullets()
    {
        orbitingBullets = new GameObject[bulletCount];
        orbitAngleOffset = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Collider2D>().enabled = true;

            orbitingBullets[i] = bullet;
        }
    }

    private void UpdateOrbitingBullets()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (Time.time * orbitSpeed + i * orbitAngleOffset) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbitRadius;
            if (orbitingBullets[i] != null)
            {
                orbitingBullets[i].transform.position = transform.position + offset;
            }
        }
    }

    private void OnDestroy()
    {
        if (orbitingBullets != null)
        {
            foreach (var b in orbitingBullets)
            {
                if (b != null) Destroy(b);
            }
        }
    }

    private void OnDrawGizmosSelected()
{
    // Спочатку викликаємо Gizmos базового класу, якщо він є
    base.OnDrawGizmosSelected();

    // Додаємо коло навколо ворога — радіус орбіти
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, orbitRadius);
}
}

