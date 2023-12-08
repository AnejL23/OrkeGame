using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject fireballPrefab; // Assign a prefab of the fireball in the inspector
    public Transform firePoint; // Assign a point from where fireballs will be shot
    public float fireRate = 3f; // Time between shots
    public float shootingDuration = 20f; // Duration of the shooting ability

    private float nextFireTime = 0f;
    private bool canShoot = false;

    public void EnableShooting()
    {
        canShoot = true;
        StartCoroutine(ShootingTimer());
    }

    private IEnumerator ShootingTimer()
    {
        yield return new WaitForSeconds(shootingDuration);
        canShoot = false;
    }

    void Update()
    {
        if (canShoot && Time.time > nextFireTime && Input.GetKeyDown(KeyCode.F))
        {
            nextFireTime = Time.time + fireRate;
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }
}
