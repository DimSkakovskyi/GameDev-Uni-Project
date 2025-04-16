using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoting : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform FirePoint;
    public FlyingChasingEnemy enemy;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (enemy != null && enemy.isChasing)
        {
            Debug.Log("Enemy is chasing!");
            Shoot();
        }
    }

    void Shoot()
    {
        //shoting logic
        Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);

    }
}
